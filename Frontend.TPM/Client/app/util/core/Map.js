Ext.define('App.util.core.Map', {
    alternateClassName: 'App.Map',

    statics: {

        reverse: function (object, valueSelector, allowMultiValue) {
            var reversed = Object.create(null),
                propertyName;

            if (Ext.isString(valueSelector) && !Ext.isEmpty(valueSelector)) {
                propertyName = valueSelector;
                valueSelector = function (k, v) {
                    return v[propertyName];
                }
            } else if (Ext.isFunction(valueSelector)) {
                valueSelector = valueSelector || Ext.identityFn;
            } else {
                valueSelector = Ext.identityFn
            }

            Ext.Object.each(object, function (key, values) {
                values = Ext.callback(valueSelector, this, [key, values]);
                reversed = Ext.Array.from(values).reduce(function (result, value) {
                    if (!!allowMultiValue) {
                        if (!result[value]) {
                            result[value] = Ext.Array.from(key, true);
                        } else {
                            result[value].push(key);
                        }
                    } else {
                        if (!result[value]) {
                            result[value] = key;
                        }
                    }

                    return result;
                }, reversed);
            }, this);

            return reversed;
        },

        pull: function (object, keys, valueSelector, onlyDefined) {
            var me = this;

            if (!Ext.isFunction(valueSelector)) {
                valueSelector = Ext.identityFn;
            }

            return Ext.Array.from(keys).reduce(function (result, key) {
                if (!onlyDefined || object[key]) {
                    result[key] = Ext.callback(valueSelector, me, [object[key]]);
                }

                return result;
            }, {});
        },

        getKeys: function (object, fn) {
            var result = [];

            fn = fn || function () {
                return true;
            }

            Ext.Object.each(object, function (key, value) {
                if (Ext.callback(fn, this, [key, value])) {
                    result.push(key);
                }
            }, this);

            return result;
        }

    }

});