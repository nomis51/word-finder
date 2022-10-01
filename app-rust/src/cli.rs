use std::collections;

pub struct Cli {
    french_words: collections::HashMap<String, u8>,
}

impl Cli {
    pub fn new() -> Cli {
        return Cli {
            french_words: collections::HashMap::new(),
        };
    }

    pub fn run(&mut self) {
        self.load_words();
    }

    fn find_words(letters: String, wordLength: u32, template: String) -> Vec<&str> {
        let results = vec![""];

        return results;
    }

    fn load_words(&mut self) -> bool {
        let french_words_path = std::fs::canonicalize("./assets/french-words.json")
            .expect("french words json not found");

        let data =
            std::fs::read_to_string(french_words_path).expect("unable to read french words json");

        let words: Vec<String> =
            serde_json::from_str(data.as_str().trim()).expect("unable to parse french words json");

        for word in words {
            self.french_words.insert(word, 0);
        }

        return true;
    }

    fn find_permutations(&self, letters: String) -> Vec<&str> {}

    fn permute(&self, results: &[&str], value: &str, letters: [char]) {
        loop {
            if letters.len() == 0 {
                if value.len() > 0 {
                    //TODO:
                }
                return;
            }

            self.permute(results, value, letters[1..]);
            value += letters[1];
            letters = letters[1..];
        }
    }

    fn arraySwap(&self, array: &[&str], i: usize, j: usize) {
        if i == j || i > array.len() || j > array.len() || i < 0 || j < 0 {
            return;
        }

//
    }
}
