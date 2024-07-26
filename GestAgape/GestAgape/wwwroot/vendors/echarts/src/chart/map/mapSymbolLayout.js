define(function (require) {

    var zrUtil = require('zrender/core/util');

    return function (ecModel) {

        var processedMapType = {};

        ecModel.eachFilieresByType('map', function (mapFilieres) {
            var mapType = mapFilieres.get('map');
            if (processedMapType[mapType]) {
                return;
            }

            var mapSymbolOffsets = {};

            zrUtil.each(mapFilieres.seriesGroup, function (subMapFilieres) {
                var geo = subMapFilieres.coordinateSystem;
                var data = subMapFilieres.getData();
                if (subMapFilieres.get('showLegendSymbol') && ecModel.getComponent('legend')) {
                    data.each('value', function (value, idx) {
                        var name = data.getName(idx);
                        var region = geo.getRegion(name);

                        // No region or no value
                        // In MapFilieres data regions will be filled with NaN
                        // If they are not in the series.data array.
                        // So here must validate if value is NaN
                        if (!region || isNaN(value)) {
                            return;
                        }

                        var offset = mapSymbolOffsets[name] || 0;

                        var point = geo.dataToPoint(region.center);

                        mapSymbolOffsets[name] = offset + 1;

                        data.setItemLayout(idx, {
                            point: point,
                            offset: offset
                        });
                    });
                }
            });

            // Show label of those region not has legendSymbol(which is offset 0)
            var data = mapFilieres.getData();
            data.each(function (idx) {
                var name = data.getName(idx);
                var layout = data.getItemLayout(idx) || {};
                layout.showLabel = !mapSymbolOffsets[name];
                data.setItemLayout(idx, layout);
            });

            processedMapType[mapType] = true;
        });
    };
});