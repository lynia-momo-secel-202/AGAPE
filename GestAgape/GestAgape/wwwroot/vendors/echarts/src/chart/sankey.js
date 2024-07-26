define(function (require) {

    var echarts = require('../echarts');

    require('./sankey/SankeyFilieres');
    require('./sankey/SankeyView');
    echarts.registerLayout(require('./sankey/sankeyLayout'));
    echarts.registerVisualCoding('chart', require('./sankey/sankeyVisual'));
});