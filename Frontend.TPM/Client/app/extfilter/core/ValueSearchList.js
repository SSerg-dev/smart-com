Ext.define('App.extfilter.core.ValueSearchList', {
    delimiter: ';',
    formatTpl: null,

    constructor: function (values, valueField, delimiter) {
        this.delimiter = delimiter || this.delimiter;
        this.formatTpl = new Ext.XTemplate('<tpl for="." between="' + this.delimiter + '">{.}</tpl>');

        this.values = Ext.Array.unique(Ext.Array.from(values).map(function (item) {
            if (item.isModel) {
                return {
                    model: item,
                    value: item.get(valueField)
                };
            } else {
                return {
                    model: null,
                    value: item
                };
            }
        }));
    },

    toString: function (itemRenderer, scope) {
        return this.toStringInternal(this.getValues(), itemRenderer, scope);
    },

    toStringByField: function (field, itemRenderer, scope) {
        var items = this.values.map(function (val) {
            if (val.model) {
                return val.model.get(field);
            } else {
                return val.value;
            }
        });

        return this.toStringInternal(items, itemRenderer, scope);
    },

    toStringInternal: function (items, itemRenderer, scope) {
        if (itemRenderer && Ext.isFunction(itemRenderer)) {
            items = items.map(itemRenderer, scope || this);
        }

        return this.formatTpl.apply(items);
    },

    getRecords: function () {
        return Ext.Array.unique(Ext.Array.clean(Ext.Array.pluck(this.values, 'model')));
    },

    getValues: function () {
        return Ext.Array.unique(Ext.Array.pluck(this.values, 'value'));
    },

    update: function (values) {
        values = Ext.Array.from(values);

        var curValues = this.getValues(),
            toInsert = Ext.Array.difference(values, curValues),
            toRemove = Ext.Array.difference(curValues, values);

        if (!Ext.isEmpty(curValues)) {
            this.values = this.values.filter(function (item) {
                return !Ext.Array.contains(toRemove, item.value);
            }, this).concat(toInsert.map(function (item) {
                return {
                    model: null,
                    value: item
                };
            }));
        }
    },

    clone: function () {
        var clone = Ext.create('App.extfilter.core.ValueSearchList', null, null, this.delimiter);
        clone.values = Ext.clone(this.values);
        return clone;
    }

});