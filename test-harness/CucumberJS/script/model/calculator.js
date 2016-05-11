'use strict';

var calculator = {
  enter: function(item) {
    this.list.push(parseInt(item));
  },
  add: function() {
    var index;
    for	(index = 0; index < this.list.length; index++) {
       this.result = this.result + this.list[index];
     }
   },
};

module.exports = {
  create: function() {
    return Object.create(calculator, {
      'list': {
        value: [],
        writable: false,
        enumerable: true
      },
      'result': { value: 0, writable: true }
    });
  }
};
