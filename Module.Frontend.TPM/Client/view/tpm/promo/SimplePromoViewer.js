Ext.define('App.view.tpm.promo.SimplePromoViewer', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.simplepromoviewer',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),

    dockedItems: [],
    systemHeaderItems: [],
    customHeaderItems: [],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            autoLoad: false,
            pageSize: 2000,
            model: 'App.model.tpm.promo.SimplePromo',
            storeId: 'simplepromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.SimplePromo',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            listeners: {
                load: function () {
                    var store = Ext.ComponentQuery.query('simplepromoviewer grid')[0].getStore();
                    var displayItem = Ext.ComponentQuery.query('simplepromoviewer #displayItem')[0],
                        msg = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), store.data.length);

                    if (displayItem) {
                        displayItem.setText(msg);
                    }
                }
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }],
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: false
            },
            items: [{
                text: l10n.ns('tpm', 'SimplePromo').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                text: l10n.ns('tpm', 'SimplePromo').value('Name'),
                dataIndex: 'Name',
                width: 150
            }, {
                text: l10n.ns('tpm', 'SimplePromo').value('BrandTechName'),
                dataIndex: 'BrandTechName',
                width: 120
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'SimplePromo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'SimplePromo').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'SimplePromo').value('DispatchesStart'),
                dataIndex: 'DispatchesStart',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'SimplePromo').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'SimplePromo').value('PromoStatusName'),
                dataIndex: 'PromoStatusName',
                width: 120
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupportpromo.PromoSupportPromoView',
        items: []
    }]
});