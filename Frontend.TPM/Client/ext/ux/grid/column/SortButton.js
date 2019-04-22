Ext.define('Ext.ux.grid.column.SortButton', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.sortbutton',

    buttonCls: Ext.baseCSSPrefix + 'column-sort-button',
    defaultIconCls: 'mdi mdi-18px',
    withoutSortIconCls: 'mdi-sort',
    ascIconCls: 'mdi-sort-ascending',
    descIconCls: 'mdi-sort-descending',

    init: function (column) {
        var me = this;

        me.column = column;

        if (!column.sortable) {
            return;
        }

        if (!column.rendered) {
            column.on('beforerender', me.onBeforeColumnRender, me, { single: true });
            column.on('afterrender', me.onColumnRender, me, { single: true });
        } else {
            me.onColumnRender();
        }

        column.on({
            destroy: me.onColumnDestroy,
            scope: me
        });
    },

    onBeforeColumnRender: function () {
        var me = this;

        me.column.getOwnerHeaderCt().on({
            sortchange: me.onSortChange,
            scope: me
        });
    },

    onColumnRender: function (column) {
        var me = this,
			btn;

        btn = me.sortButtonEl = me.column.titleEl.createChild({
            tag: 'div',
            cls: [me.buttonCls, me.defaultIconCls, me.withoutSortIconCls].join(' ')
        });

        //btn.on('click', me.onButtonClick, me);
    },

    onColumnDestroy: function () {
        this.sortButtonEl.destroy();
    },

    onSortChange: function (header, column, direction) {
        var el = this.sortButtonEl,
            defaultCls = this.withoutSortIconCls,
            ascCls = this.ascIconCls,
            descCls = this.descIconCls;

        if (this.column.dataIndex === column.dataIndex) {
            switch (direction) {
                case 'ASC':
                    el.addCls(ascCls);
                    el.removeCls([defaultCls, descCls]);
                    break;
                case 'DESC':
                    el.addCls(descCls);
                    el.removeCls([defaultCls, ascCls]);
                    break;
            }
        } else {
            el.addCls(defaultCls);
            el.removeCls([ascCls, descCls]);
        }
    }//,

    //onButtonClick: function (e) {
    //    this.column.toggleSortState();
    //    e.stopEvent();
    //}
});
