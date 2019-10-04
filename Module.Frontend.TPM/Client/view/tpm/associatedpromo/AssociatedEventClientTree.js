Ext.define('App.view.tpm.associatedpromo.AssociatedEventClientTree', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedeventclienttree',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'event',
        minHeight: 150,
        flex: 1,
        suppressSelection: true,
        linkConfig: {
            'eventclienttree': { masterField: 'Id', detailField: 'EventId' }
        }
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'eventclienttree',
        minHeight: 150,
        flex: 1,
        autoLoad: false,
    }]
});