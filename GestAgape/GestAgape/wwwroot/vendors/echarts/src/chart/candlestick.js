define(function (require) {

    var echarts = require('../echarts');

    require('./candlestick/CandlestickFilieres');
    require('./candlestick/CandlestickView');

    echarts.registerPreprocessor(
        require('./candlestick/preprocessor')
    );

    echarts.registerVisualCoding('chart', require('./candlestick/candlestickVisual'));
    echarts.registerLayout(require('./candlestick/candlestickLayout'));

});