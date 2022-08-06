const { findWords } = require('./functions');

if (process.argv.length < 3 || process.argv[2].length <= 0) return console.log('Please specify some letters');

const letters = process.argv[2];
const length = process.argv.length < 4 ? letters.length : Number.isInteger(Number(process.argv[3])) ? Number(process.argv[3]) : letters.length;
const template = process.argv.length < 5 || process.argv[4].length !== length ? '' : process.argv[4];

console.log('Letters: ', letters);
console.log('Length: ', length);
if (template) {
	console.log(
		'Template: ',
		template.split('')
			.map(c => {
				if (c === '-') return '_';
				return c;
			})
			.join(' ')
	);
}

const words = findWords(letters, length, template.split(''));
console.log('Words: ', words);
