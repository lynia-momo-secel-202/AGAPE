define(function (require) {

    var echarts = require('../echarts');

    require('./treemap/TreemapFilieres');
    require('./treemap/TreemapView');
    require('./treemap/treemapAction');

    echarts.registerVisualCoding('chart', require('./treemap/treemapVisual'));

    echarts.registerLayout(require('./treemap/treemapLayout'));
});