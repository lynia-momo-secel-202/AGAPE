define(function (require) {

    var zrUtil = require('zrender/core/util');
    var geoProps = [
        'x', 'y', 'x2', 'y2', 'width', 'height', 'map', 'roam', 'center', 'zoom', 'scaleLimit', 'label', 'itemStyle'
    ];

    var geoCoordsMap = {};

    function createGeoFromMap(mapFilieresOpt) {
        var geoOpt = {};
        zrUtil.each(geoProps, function (propName) {
            if (mapFilieresOpt[propName] != null) {
                geoOpt[propName] = mapFilieresOpt[propName];
            }
        });
        return geoOpt;
    }
    return function (option) {
        // Save geoCoord
        var mapFilieres = [];
        zrUtil.each(option.series, function (seriesOpt) {
            if (seriesOpt.type === 'map') {
                mapFilieres.push(seriesOpt);
            }
            zrUtil.extend(geoCoordsMap, seriesOpt.geoCoord);
        });

        var newCreatedGeoOptMap = {};
        zrUtil.each(mapFilieres, function (seriesOpt) {
            seriesOpt.map = seriesOpt.map || seriesOpt.mapType;
            // Put x, y, width, height, x2, y2 in the top level
            zrUtil.defaults(seriesOpt, seriesOpt.mapLocation);
            if (seriesOpt.markPoint) {
                var markPoint = seriesOpt.markPoint;
                // Convert name or geoCoord in markPoint to lng and lat
                // For example
                // { name: 'xxx', value: 10} Or
                // { geoCoord: [lng, lat], value: 10} to
                // { name: 'xxx', value: [lng, lat, 10]}
                markPoint.data = zrUtil.map(markPoint.data, function (dataOpt) {
                    if (!zrUtil.isArray(dataOpt.value)) {
                        var geoCoord;
                        if (dataOpt.geoCoord) {
                            geoCoord = dataOpt.geoCoord;
                        }
                        else if (dataOpt.name) {
                            geoCoord = geoCoordsMap[dataOpt.name];
                        }
                        var newValue = geoCoord ? [geoCoord[0], geoCoord[1]] : [NaN, NaN];
                        if (dataOpt.value != null) {
                            newValue.push(dataOpt.value);
                        }
                        dataOpt.value = newValue;
                    }
                    return dataOpt;
                });
                // Convert map series which only has markPoint without data to scatter series
                // FIXME
                if (!(seriesOpt.data && seriesOpt.data.length)) {
                    if (!option.geo) {
                        option.geo = [];
                    }
                    else if (!zrUtil.isArray(option.geo)) {
                        option.geo = [option.geo];
                    }

                    // Use same geo if multiple map series has same map type
                    var geoOpt = newCreatedGeoOptMap[seriesOpt.map];
                    if (!geoOpt) {
                        geoOpt = newCreatedGeoOptMap[seriesOpt.map] = createGeoFromMap(seriesOpt);
                        option.geo.push(geoOpt);
                    }

                    var scatterFilieres = seriesOpt.markPoint;
                    scatterFilieres.type = option.effect && option.effect.show ? 'effectScatter' : 'scatter';
                    scatterFilieres.coordinateSystem = 'geo';
                    scatterFilieres.geoIndex = zrUtil.indexOf(option.geo, geoOpt);
                    scatterFilieres.name = seriesOpt.name;

                    option.series.splice(zrUtil.indexOf(option.series, seriesOpt), 1, scatterFilieres);
                }
            }
        });
    };
});