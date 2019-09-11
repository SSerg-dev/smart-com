Ext.define('App.view.core.common.GridInfoToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.gridinfotoolbar',
    ui: 'light-gray-toolbar',

    mixins: {
        bindable: 'Ext.util.Bindable'
    },

    displayMsg: l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'),

    items: [{
        xtype: 'tbtext',
        ui: 'grid-info-text',
        itemId: 'displayItem'
    }],

    initComponent: function () {
        var me = this;

        me.bindStore(me.store || 'ext-empty-store', true);
        me.callParent();
    },

    beforeRender: function () {
        this.callParent(arguments);
        if (!this.store.isLoading()) {
            this.onLoad();
        }
    },

    onLoad: function () {
        var me = this,
            displayItem = me.child('#displayItem'),
            totalCount = me.store.pageSize ? me.store.getTotalCount() : me.store.getCount(),
            msg = Ext.String.format(me.displayMsg, totalCount);

        if (displayItem) {
            displayItem.setText(msg);
        }
    },

    getStoreListeners: function () {
        return {
            load: this.onLoad,
            prefetch: this.onLoad
        };
    },

    unbind: function (store) {
        this.bindStore(null);
    },

    bind: function (store) {
        this.bindStore(store);
    },

    onDestroy: function () {
        this.unbind();
        this.callParent();
    }
});