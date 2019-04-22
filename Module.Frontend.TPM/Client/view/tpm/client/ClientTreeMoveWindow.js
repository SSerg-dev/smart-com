Ext.define('App.view.tpm.client.ClientTreeMoveWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.clienttreemovewindow',

    autoScroll: false,
    cls: 'promoform',
    width: 500,
    minWidth: 500,
    minHeight: 480,
    maxHeight: 400,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        flex: 1,
        margin: '10 8 15 15'
    },

    title: 'Node moving',

    initComponent: function () {
        this.callParent(arguments);
    },

    items: [{
        xtype: 'clienttree',
        title: 'Select destination node',
        minHeight: 350,
        dockedItems: [],
        items: [{
            xtype: 'clienttreegrid',
            header: {
                //titlePosition: 0,
                height: 40,
                defaults: {
                    xtype: 'button',
                    ui: 'white-button',
                    padding: 1 //TODO: временно
                },
                items: [{
                    triggerCls: Ext.baseCSSPrefix + 'form-clear-trigger',
                    xtype: 'trigger',
                    hideLabel: true,
                    editable: true,
                    cls: 'tree-search-text-def',
                    onTriggerClick: function () {
                        var me = this;
                        me.setRawValue('Client search');
                        me.addClass('tree-search-text-def');
                        var store = me.up('basetreegrid').getStore();
                        var proxy = store.getProxy();
                        proxy.extraParams.filterParameter = null;
                        store.load();
                        me.triggerBlur();
                        me.blur();
                    },

                    listeners: {
                        afterrender: function (field) {
                            field.setRawValue('Client search');
                        },
                        focus: function (field) {
                            if (field.getRawValue() == 'Client search') {
                                field.setRawValue('');
                                field.removeCls('tree-search-text-def');
                            }
                        },
                        blur: function (field) {
                            if (field.getRawValue() == '') {
                                field.setRawValue('Client search');
                                field.addClass('tree-search-text-def');
                            }
                        },
                        specialkey: function (field, e) {
                            if (e.getKey() == e.ENTER) {
                                var value = field.getValue(),
                                    treegrid = field.up('basetreegrid'),
                                    store = treegrid.store;
                                store.getProxy().extraParams.filterParameter = value;
                                store.load();
                            }
                        }
                    }
                }]
            },
            store: {
                storeId: 'clienttreemovestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: true,
                root: {}
            },
        }]
    }]
})