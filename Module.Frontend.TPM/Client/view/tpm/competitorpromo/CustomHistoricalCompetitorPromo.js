Ext.define('App.view.tpm.competitorpromo.CustomHistoricalCompetitorPromo', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.customhistoricalcompetitorpromo',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    dockedItems: [{
        xtype: 'editorform',
        cls: 'hierarchydetailform',
        columnsCount: 1,
        dock: 'right',
        width: 400,
        layout: 'fit',

        items: [{
            xtype: 'custompromopanel',
            margin: 2,
            overflowY: 'auto',
            height: 480,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'fieldset',
                title: 'User info',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                defaults: {
                    padding: '0 3 0 3',
                },
                items: [{
                    xtype: 'singlelinedisplayfield',
                    name: '_User',
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_User')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: '_Role',
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_Role')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: '_EditDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_EditDate')
                }, {
                    xtype: 'singlelinedisplayfield',
                    name: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromo', 'OperationType'),
                    fieldLabel: l10n.ns('tpm', 'HistoricalPromo').value('_Operation')
                }]
            }, {
                xtype: 'fieldset',
                title: 'Change info',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                defaults: {
                    padding: '0 3 0 3',
                    labelWidth: 200
                },
                items: [
                    { xtype: 'singlelinedisplayfield', name: 'PromoStatusName', fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'ClientHierarchy', fieldLabel: l10n.ns('tpm', 'Promo').value('ClientHierarchy'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MarsMechanicName', fieldLabel: l10n.ns('tpm', 'Promo').value('MarsMechanicName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MarsMechanicTypeName', fieldLabel: l10n.ns('tpm', 'Promo').value('MarsMechanicTypeName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MarsMechanicDiscount', fieldLabel: l10n.ns('tpm', 'Promo').value('MarsMechanicDiscount'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInstoreMechanicName', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInstoreMechanicTypeName', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicTypeName'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'PlanInstoreMechanicDiscount', fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscount'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'MechanicComment', fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicComment'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'StartDate', fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'), renderer: Ext.util.Format.dateRenderer('d.m.Y'), hidden: true },
                    { xtype: 'singlelinedisplayfield', name: 'EndDate', fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'), renderer: Ext.util.Format.dateRenderer('d.m.Y'), hidden: true },
                ]
            }]
        }]
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        flex: 3,

        store: {
            type: 'directorystore',
            model: 'App.model.tpm.competitorpromo.HistoricalCompetitorPromo',
            storeId: 'customhistoricalcompetitorpromostore',
            autoLoad: false,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.HistoricalCommpetitorPromo',
                    modelId: 'efselectionmodel'
                }]
            },
            sorters: [{
                property: '_EditDate',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 2,
                minWidth: 100
            },

            items: [{
                text: l10n.ns('tpm', 'HistoricalPromo').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPromo').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalPromo').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalPromo').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromo', 'OperationType'),
                filter: {
                    type: 'combo',
                    valueField: 'id',
                    store: {
                        type: 'operationtypestore'
                    },
                    operator: 'eq'
                }
            }]
        },

        listeners: {
            select: function (cell, record) {

                form = this.up().down('editorform');
                var fields = form.getForm().getFields();

                Ext.suspendLayouts();
                fields.each(function (item, index) {
                    var changedValue = record.get(item.name);
                    if (changedValue != null) {
                        item.setValue(changedValue);
                        item.show();
                    } else {
                        item.hide();
                    }
                });
                Ext.resumeLayouts(true);
            }
        }
    }]
});
