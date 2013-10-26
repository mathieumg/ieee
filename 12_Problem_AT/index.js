// Modules.
var util = require('util');

function processData(input) {
    var problemData = input.split(' '),
        inputNumber = parseInt(problemData[0], 10),
        a = inputNumber,
        s = a,
        b = a,
        c = 0,
        d = 0,
        carry = 0,
        extra = 0;

    while (a !== 0) {
        if (a % 2 !== 0) {
          carry = 1;
        } else {
          carry = 0;
        }

        a >>= 1;

        c <<= 1;
        c += carry;
    };

    a = c;
    c = 0;

    d = 513; //0212h

    while (Math.floor(d / 256) <= b) {
      if(d >= Math.pow(2, 31)) {
        carry = 1;
      } else {
        carry = 0;
      }

      d *= 2;
      d += carry;


      if (b % 2 !== 0) {
        carry = 1;
      } else {
        carry = 0;
      }

      b /= 2;
      b = Math.floor(b);

      c *= 2;
      c += carry;
    }

    if (b > d) {
      if (b % 2 !== 0) {
        carry = 1;
        extra = Math.pow(2, 31);
      } else {
        carry = 0;
      }

      b /= 2;
      b = Math.floor(b) + extra;

      c *= 2;
      c += carry;

      if(b >= Math.pow(2, 31)) {
        carry = 1;
      } else {
        carry = 0;
      }

      b *= 2;
      b += carry;
    }

    a = a - d;
    d -= 1;
    a &= d;
    d &= s;

    if (a > d) {
      carry = 1;
    } else {
      carry = 0;
    }

    b = b - a - carry;

    console.log(b);
} 

process.stdin.resume();
process.stdin.setEncoding("ascii");
_input = "";
process.stdin.on("data", function (input) {
    _input += input;
});

process.stdin.on("end", function () {
   processData(_input.replace(/(\r\n|\n|\r)/gm,''));
});
