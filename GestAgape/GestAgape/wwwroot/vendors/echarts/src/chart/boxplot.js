define(function (require) {

    var echarts = require('../echarts');

    require('./boxplot/BoxplotFilieres');
    require('./boxplot/BoxplotView');

    echarts.registerVisualCoding('chart', require('./boxplot/boxplotVisual'));
    echarts.registerLayout(require('./boxplot/boxplotLayout'));

});