Ext.define('App.extfilter.core.ValueList', {

    formatTpl: new Ext.XTemplate(
        '<tpl for="." between=", ">',
            '<tpl if="Ext.isEmpty(values)">',
                '{[l10n.ns("core", "multiSelect").value("emptyText")]}',
            '<tpl elseif="Ext.isString(values)">',
                '\'{.}\'',
            '<tpl else>',
                '{[values.toString()]}',
            '</tpl>',
        '</tpl>'
    ),

    constructor: function (values) {
        this.values = values;
    },

    toString: function (itemRenderer, scope) {
        var items = Ext.Array.from(this.values);

        if (itemRenderer && Ext.isFunction(itemRenderer)) {
            items = items.map(itemRenderer, scope || this);
        }

        return this.formatTpl.apply(items);
    }

});