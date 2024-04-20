import './style.css';
import { Words } from './words'

const buttonSearchElement = document.getElementById('button-search') as HTMLButtonElement;
const inputLettersElement = document.getElementById('input-letters') as HTMLInputElement;
const inputLengthElement = document.getElementById('input-length') as HTMLInputElement;
const inputTemplateElement = document.getElementById('input-template') as HTMLInputElement;
const selectLanguagesElement = document.getElementById('select-language') as HTMLSelectElement;
const divWordsListElement = document.getElementById('words-list') as HTMLDivElement;

buttonSearchElement.addEventListener('click', findWords);

const VALID_LANGUAGES = ['french', 'english'];

function findWords() {
	const letters = inputLettersElement.value;
	const lengthStr = parseInt(inputLengthElement.value);
	const template = inputTemplateElement.value;
	const language = selectLanguagesElement.value;

	let length = 0;

	if (template) {
		length = template.length;
		inputLengthElement.value = length.toString();
	} else {
		length = Number(lengthStr);
	}

	if (letters.length === 0) {
		divWordsListElement.innerHTML = 'No letters provided';
		return;
	}

	if (length < 0 || isNaN(length)) {
		divWordsListElement.innerHTML = 'No length or template provided';
		return;
	}

	if (!VALID_LANGUAGES.includes(language)) {
		divWordsListElement.innerHTML = 'Invalid language selected';
		return;
	}

	const permutations = getPermutations(letters);

	const words: Set<string> = new Set();

	for (var permutation of permutations) {
		if (permutation.length !== length) continue;

		if (template) {
			let match = true;

			for (let i = 0; i < template.length; ++i) {
				if (template[i] === '-') continue;
				if (i < permutation.length && permutation[i] === template[i]) continue;

				match = false;
				break;
			}

			if (!match) continue;
		}

		if (!Words[language][permutation]) continue;
		if (words.has(permutation)) continue;

		words.add(permutation);
	}

	divWordsListElement.innerHTML = '';

	if (words.size === 0) {
		divWordsListElement.innerHTML = 'No words found';
	}

	for (const word of words) {
		const li = document.createElement('li');
		li.innerText = word;
		divWordsListElement.appendChild(li);
	}
}

function getPermutations(letters: string): string[] {
	const results: string[] = [];
	permute(results, "", letters.split(""));
	return results;
}

function permute(results: string[], str: string, letters: string[]) {
	while (true) {
		if (letters.length === 0) {
			if (str.length > 0) {
				permuteRecursive(results, "", str.split(""));
			}

			return;
		}

		permute(results, str, letters.slice(1));
		str += letters[0];
		letters = letters.slice(1);
	}
}

function permuteRecursive(results: string[], str: string, letters: string[]) {
	if (letters.length === 0) {
		results.push(str);
		return;
	}

	for (let i = 0; i < letters.length; ++i) {
		arraySwap(letters, 0, i);
		permuteRecursive(results, str + letters[0], letters.slice(1));
		arraySwap(letters, 0, i);
	}
}

function arraySwap(array: any[], i: number, j: number) {
	if (i === j || i > array.length || j > array.length || i < 0 || j < 0) return;

	const temp = array[i];
	array[i] = array[j];
	array[j] = temp;
}