Ext.define('App.util.core.Array', {
    alternateClassName: 'App.Array',

    statics: {

        groupBy: function (array, keySelector, valueSelector, scope) {
            var groupsMap,
                groups = [],
                propertyName;

            array = Ext.Array.from(array);

            if (Ext.isString(keySelector)) {
                propertyName = keySelector;
                keySelector = function (v) {
                    return v[propertyName];
                }
            }

            valueSelector = valueSelector || function (k, v) {
                return v;
            };

            groupsMap = array.reduce(function (result, item) {
                var key = Ext.callback(keySelector, scope, [item]) || '';
                (result[key] || (result[key] = [])).push(item);
                return result;
            }, {});

            Ext.Object.each(groupsMap, function (key, items) {
                groups.push({
                    key: key,
                    items: Ext.callback(valueSelector, scope, [key, items])
                });
            });

            return groups;
        }

    }

});