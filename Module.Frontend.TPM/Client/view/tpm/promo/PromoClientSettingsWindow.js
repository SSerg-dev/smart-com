Ext.define('App.view.tpm.promo.PromoClientSettingsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoclientsettingswindow',

    width: 600,
    minWidth: 280,
    height: '90%',
    maxHeight: 580,
    resizable: false,
    title: l10n.ns('tpm', 'PromoClient').value('Settings'), 

    items: [{
        xtype: 'panel',
        autoScroll: true,
        cls: 'scrollpanel',
        bodyStyle: { "background-color": "#ECEFF1"},
        layout: {
            type: 'vbox',
            align: 'stretch',
            pack: 'start'
        },
        items: [{
            xtype: 'custompromopanel',
            minWidth: 245,
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'center'
            },             
            items: [{
                xtype: 'fieldset',
                title: 'Parameters',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                defaults: {
                    padding: '0 3 0 3',
                },
                items: [{
                    xtype: 'singlelinedisplayfield',
                    name: 'ObjectId',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('ClientHierarchyCode'),
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'Type',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('Type'),
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'Name',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('Name'),
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'OutletCount',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('OutletCount')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'GHierarchyCode',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('GHierarchyCode')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'DemandCode',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('DemandCode')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'IsBaseClient',
                    renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('IsBaseClient')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'RetailTypeName',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'RetailType').value('Name')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: 'DistrMarkUp',
                    width: 280,
                    fieldLabel: l10n.ns('tpm', 'ClientTree').value('DistrMarkUp')
                }]
            }, {
                xtype: 'fieldset',
                cls: 'dispatch-settings-fieldset',
                title: l10n.ns('tpm', 'Dispatch').value('DispatchTitle'),
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                defaults: {
                    padding: '1 3 1 3',
                    minHeight: 55
                },
                items: [{
                    xtype: 'fieldcontainer',
                    cls: 'dispatchsettingsfieldcontainer',
                    itemId: 'fieldcontainerStart',
                    fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchStart'),
                    labelAlign: 'top',
                    labelSeparator: ' ',
                    width: '100%',
                    labelWidth: 35,
                    layout: {
                        type: 'hbox',
                        align: 'middle',
                        pack: 'center'
                    },
                    items: [{
                        xtype: 'booleancombobox',
                        cls: 'hierarchydetailfield',
                        name: 'IsBeforeStart',
                        store: {
                            type: 'booleanstore',
                            data: [
                                { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                                { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                            ]
                        },
                        flex: 2,
                        style: 'margin-right: 5px;',
                        readOnly: true
                    }, {
                        xtype: 'numberfield',
                        cls: 'hierarchydetailfield',
                        name: 'DaysStart',
                        minValue: 1,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        allowDecimals: false,
                        flex: 1,
                        style: 'margin-right: 5px',
                        readOnly: true
                    }, {
                        xtype: 'booleancombobox',
                        cls: 'hierarchydetailfield',
                        name: 'IsDaysStart',
                        store: {
                            type: 'booleanstore',
                            data: [
                                { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                                { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                            ],
                        },
                        flex: 2,
                        readOnly: true
                    }]
                }, {
                    xtype: 'fieldcontainer',
                    cls: 'dispatchsettingsfieldcontainer',
                    itemId: 'fieldcontainerEnd',
                    fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchEnd'),
                    labelAlign: 'top',
                    labelSeparator: ' ',
                    width: '100%',
                    labelWidth: 35,
                    layout: {
                        type: 'hbox',
                        align: 'middle'
                    },
                    style: 'margin-bottom: 0',
                    items: [{
                        xtype: 'booleancombobox',
                        cls: 'hierarchydetailfield',
                        name: 'IsBeforeEnd',
                        store: {
                            type: 'booleanstore',
                            data: [
                                { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                                { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                            ]
                        },
                        flex: 2,
                        style: 'margin-right: 5px',
                        readOnly: true
                    }, {
                        xtype: 'numberfield',
                        cls: 'hierarchydetailfield',
                        name: 'DaysEnd',
                        minValue: 1,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        allowDecimals: false,
                        flex: 1,
                        style: 'margin-right: 5px;',
                        readOnly: true
                    }, {
                        xtype: 'booleancombobox',
                        cls: 'hierarchydetailfield',
                        name: 'IsDaysEnd',
                        store: {
                            type: 'booleanstore',
                            data: [
                                { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                                { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                            ]
                        },
                        flex: 2,
                        readOnly: true
                    }]
                }]
            }],               
        }]
    }],

    fillForm: function (record) {
        this.down('[name=ObjectId]').setValue(record.ObjectId);
        this.down('[name=Type]').setValue(record.Type);
        this.down('[name=Name]').setValue(record.Name);
        //'OutletCount',
        this.down('[name=GHierarchyCode]').setValue(record.GHierarchyCode);
        this.down('[name=DemandCode]').setValue(record.DemandCode); 
        this.down('[name=IsBaseClient]').setValue(record.IsBaseClient);
        this.down('[name=RetailTypeName]').setValue(record.RetailTypeName);
        this.down('[name=DistrMarkUp]').setValue(record.DistrMarkUp);
        this.down('[name=PostPromoEffectW1]').setValue(record.PostPromoEffectW1);
        this.down('[name=PostPromoEffectW2]').setValue(record.PostPromoEffectW2);

        this.down('[name=IsBeforeStart]').setValue(record.IsBeforeStart);
        this.down('[name=DaysStart]').setValue(record.DaysStart);
        this.down('[name=IsDaysStart]').setValue(record.IsDaysStart);
        this.down('[name=IsBeforeEnd]').setValue(record.IsBeforeEnd);
        this.down('[name=DaysEnd]').setValue(record.DaysEnd);
        this.down('[name=IsDaysEnd]').setValue(record.IsDaysEnd);
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }]
})