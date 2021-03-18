Ext.define('App.view.tpm.ratishopper.DeletedRATIShopper', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedratishopper',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.ratishopper.DeletedRATIShopper',
            storeId: 'deletedratishopperstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.ratishopper.DeletedRATIShopper',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [
                {
                    text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                    dataIndex: 'DeletedDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'RATIShopper').value('ClientTreeFullPathName'),
                    dataIndex: 'ClientTreeFullPathName',
                    minWidth: 200,
                    filter: {
                        xtype: 'treefsearchfield',
                        trigger2Cls: '',
                        selectorWidget: 'clienttree',
                        valueField: 'FullPathName',
                        displayField: 'FullPathName',
                        multiSelect: true,
                        operator: 'conts',
                        store: {
                            model: 'App.model.tpm.clienttree.ClientTree',
                            autoLoad: false,
                            root: {}
                        },
                    },
                    renderer: function (value) {
                        return renderWithDelimiter(value, ' > ', '  ');
                    }
                }, {
                    text: l10n.ns('tpm', 'RATIShopper').value('ClientTreeObjectId'),
                    dataIndex: 'ClientTreeObjectId'
                }, {
                    text: l10n.ns('tpm', 'RATIShopper').value('Year'),
                    dataIndex: 'Year'
                }, {
                    text: l10n.ns('tpm', 'RATIShopper').value('RATIShopperPercent'),
                    dataIndex: 'RATIShopperPercent'
                }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.ratishopper.DeletedRATIShopper',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Year',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('Year')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'RATIShopperPercent',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('RATIShopperPercent')
        }]
    }]
});


