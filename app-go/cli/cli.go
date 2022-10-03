package cli

import (
	"encoding/json"
	"fmt"
	"os"
	"path"
	"strconv"
	"strings"
)

type Cli struct {
	frenchWords map[string]uint8
}

func (cli *Cli) Run() {
	cli.loadWords()

	for {
		fmt.Print("Specify the letters: ")
		var letters string
		_, errLetters := fmt.Scanln(&letters)
		if errLetters != nil || len(letters) == 0 {
			continue
		}

		fmt.Print("Specify a word template or a word length: ")
		var templateOrLength string
		_, errTemplateOrLength := fmt.Scanln(&templateOrLength)
		if errTemplateOrLength != nil || len(templateOrLength) == 0 {
			continue
		}

		var template string = ""
		wordLength, _ := strconv.Atoi(templateOrLength)
		if wordLength == 0 {
			if strings.Contains(templateOrLength, "-") {
				template = templateOrLength
			} else {
				continue
			}
		}

		words := cli.findWords(letters, template, wordLength)
		if len(words) == 0 {
			fmt.Println("No words found")
		}

		fmt.Println("Possible words: ")
		for _, word := range words {
			fmt.Println(word)
		}

		fmt.Println("Press any key to continue or press 'q' to exit")
		var answer string
		fmt.Scanln(&answer)
		if answer == "q" {
			break
		}
	}
}

func (cli *Cli) findWords(letters string, template string, wordLength int) []string {
	possibleWords := cli.findPermutations(strings.Split(letters, ""))

	if len(possibleWords) == 0 {
		return []string{}
	}

	foundWords := make(map[string]uint8)

	for _, possibleWord := range possibleWords {
		if (wordLength != 0 && len(possibleWord) != wordLength) || (wordLength == 0 && len(possibleWord) != len(template)) {
			continue
		}

		if len(template) > 0 {
			wordMatchTemplate := true
			for index, char := range template {
				if char == '-' || (index < len(possibleWord) && possibleWord[index:index+1] == string(char)) {
					continue
				}

				wordMatchTemplate = false
				break
			}

			if !wordMatchTemplate {
				continue
			}
		}

		if _, ok := cli.frenchWords[possibleWord]; !ok {
			continue
		}
		if _, ok := foundWords[possibleWord]; ok {
			continue
		}

		foundWords[possibleWord] = 0
	}

	result := make([]string, 0, len(foundWords))
	for v, _ := range foundWords {
		result = append(result, v)
	}

	return result
}

func (cli *Cli) findPermutations(letters []string) []string {
	var results []string
	cli.permute(&results, "", letters)
	return results
}

func (cli *Cli) permute(results *[]string, str string, letters []string) {
	for {
		if len(letters) == 0 {
			if len(str) > 0 {
				cli.permuteRec(results, "", strings.Split(str, ""))
			}

			return
		}

		cli.permute(results, str, letters[1:])
		str += letters[0]
		letters = letters[1:]
	}
}

func (cli *Cli) permuteRec(results *[]string, str string, letters []string) {
	if len(letters) == 0 {
		*results = append(*results, str)
	} else {
		for i := 0; i < len(letters); i++ {
			cli.arraySwap(&letters, 0, i)
			cli.permuteRec(results, str+letters[0], letters[1:])
			cli.arraySwap(&letters, 0, i)
		}
	}
}

func (cli *Cli) arraySwap(array *[]string, i int, j int) {
	if i == j || i > len(*array) || j > len(*array) || i < 0 || j < 0 {
		return
	}

	var temp = (*array)[i]
	(*array)[i] = (*array)[j]
	(*array)[j] = temp
}

func (cli *Cli) loadWords() {
	cwd, _ := os.Getwd()
	var data, errReadFile = os.ReadFile(path.Join(cwd, "./assets/french-words.json"))

	if errReadFile != nil {
		panic("unable to load french words json")
	}

	var jsonData []string
	var errParseJson = json.Unmarshal(data, &jsonData)

	if errParseJson != nil {
		panic("unable to parse french words json")
	}

	cli.frenchWords = make(map[string]uint8)

	for _, word := range jsonData {
		cli.frenchWords[word] = 0
	}
}
