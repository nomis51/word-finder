const fs = require('fs');

const words = loadData();

function findWords(input, length, template = undefined) {
	const possibleWords = permute__of_all_size(input.split(''));

	const foundWords = [];
	for (const possibleWord of possibleWords) {
		if (possibleWord.length !== length) continue;

		let templateWork = true;
		for (var i = 0; i < template.length; ++i) {
			if (template[i] === '-') continue;
			if (possibleWord[i] !== template[i]) {
				templateWork = false;
				break;
			}
		}

		if (!templateWork) continue;
		if (!words.includes(possibleWord)) continue;
		if (foundWords.includes(possibleWord)) continue;

		foundWords.push(possibleWord);
	}

	return foundWords
		.sort((a, b) => a > b ? 1 : -1)
}

function loadData() {
	const wordsFilePath = "../assets/an-array-of-french-words/index.json";
	const data = fs.readFileSync(wordsFilePath, { encoding: 'utf8' });
	return JSON.parse(data);
}

function swap(array, i, j) {
	if (i != j) {
		var swap = array[i];
		array[i] = array[j];
		array[j] = swap;
	}
}

function permute_rec(res, str, array) {
	if (array.length == 0) {
		res.push(str);
	} else {
		for (var i = 0; i < array.length; i++) {
			swap(array, 0, i);
			permute_rec(res, str + array[0], array.slice(1));
			swap(array, 0, i);
		}
	}
}

function permute(array) {
	var res = [];

	permute_rec(res, "", array);
	return res;
}

function xpermute_rec(res, sub, array) {
	if (array.length == 0) {
		if (sub.length > 0) permute_rec(res, "", sub);
	} else {
		xpermute_rec(res, sub, array.slice(1));
		xpermute_rec(res, sub.concat(array[0]), array.slice(1));
	}
}

function permute__of_all_size(array) {
	var res = [];

	xpermute_rec(res, [], array);
	return res;
}

module.exports = {
	findWords
};