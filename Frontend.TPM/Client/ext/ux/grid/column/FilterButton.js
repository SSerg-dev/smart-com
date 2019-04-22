Ext.define('Ext.ux.grid.column.FilterButton', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.filterbutton',

    buttonCls: Ext.baseCSSPrefix + 'column-filter-button',
    iconCls: 'mdi mdi-18px mdi-filter-variant',

    init: function (column) {
        var me = this;

        me.column = column;

        if (!column.filter) {
            return;
        }

        if (!column.rendered) {
            column.on('afterrender', me.onColumnRender, me, { single: true });
        } else {
            me.onColumnRender();
        }

        column.on({
            destroy: me.onColumnDestroy,
            scope: me
        });
    },

    onColumnRender: function (column) {
        var me = this,
			btn;

        btn = me.filterButtonEl = me.column.titleEl.createChild({
            tag: 'div',
            cls: me.buttonCls + ' ' + me.iconCls
        });

        btn.on('click', me.onButtonClick, me);
    },

    onColumnDestroy: function () {
        this.filterButtonEl.destroy();
    },

    onButtonClick: function (e) {
        if (this.column.filter) {
            this.column.filter.clearFilter();
        }
        e.stopEvent();
    }
});
