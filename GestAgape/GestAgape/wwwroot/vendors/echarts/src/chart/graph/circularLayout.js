define(function (require) {
    var circularLayoutHelper = require('./circularLayoutHelper');
    return function (ecModel, api) {
        ecModel.eachFilieresByType('graph', function (seriesModel) {
            if (seriesModel.get('layout') === 'circular') {
                circularLayoutHelper(seriesModel);
            }
        });
    };
});