Ext.define('App.extfilter.core.RangeList', {

    formatTpl: new Ext.XTemplate(
        '<tpl for="." between=", ">',
            '<tpl if="Ext.isEmpty(from)">',
                '{[l10n.ns("core", "multiSelect").value("emptyText")]}',
            '<tpl else>',
                '{from}',
            '</tpl>',
            ' - ',
            '<tpl if="Ext.isEmpty(to)">',
                '{[l10n.ns("core", "multiSelect").value("emptyText")]}',
            '<tpl else>',
                '{to}',
            '</tpl>',
        '</tpl>.'
    ),

    constructor: function (ranges) {
        this.ranges = ranges;
    },

    toString: function (itemRenderer, scope) {
        var items = Ext.Array.from(this.ranges);

        if (itemRenderer && Ext.isFunction(itemRenderer)) {
            items = items.map(itemRenderer, scope || this);
        }

        return this.formatTpl.apply(items);
    }

});