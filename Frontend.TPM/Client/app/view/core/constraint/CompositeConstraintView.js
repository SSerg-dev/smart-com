Ext.define('App.view.core.constraint.CompositeConstraintView', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.compositeconstraint',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'userrolemain',
        margin: '10 0 0 20',
        minHeight: 150,
        suppressSelection: true,
        linkConfig: {
            'constraint': { masterField: 'Id', detailField: 'UserRoleId' }
        }
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'constraint',
        margin: '0 0 20 20',
        minHeight: 150,
        autoLoadStore: false
    }]
});