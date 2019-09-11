Ext.define('App.view.core.main.Navigation', {
    extend: 'Ext.container.Container',
    alias: 'widget.navigation',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'triggerfield',
        trigger1Cls: Ext.baseCSSPrefix + 'form-search-trigger',
        searchCls: Ext.baseCSSPrefix + 'form-search-trigger',
        clearCls: Ext.baseCSSPrefix + 'form-clear-trigger',
        itemId: 'menusearchfield',
        padding: '10 10 5 10',
        cls: 'menusearchfield',
        maxSearchResult: 8,
        isSearchActive: false,
        onTrigger1Click: function () {
            if (this.isSearchActive) {
                this.setValue('');
            }
        }
    }, {
        xtype: 'button',
        itemId: 'back',
        ui: 'main-menu-button',
        cls: 'backbutton',
        scale: 'medium',
        glyph: 0xf14c,
        textAlign: 'left',
        margin: '5 10 5 10',
        hidden: true
    }, {
        xtype: 'container',
        flex: 1,
        autoScroll: true,

        items: [{
            xtype: 'panel',
            itemId: 'menucontainer',
            ui: 'transparent-panel',
            padding: '5 0 5 0',

            layout: {
                type: 'vbox',
                align: 'stretch'
            }

        }]

    }]

});
