Ext.define('App.view.tpm.coefficientsi2so.DeletedCoefficientSI2SO', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedbrand',
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
            model: 'App.model.tpm.coefficientsi2so.DeletedCoefficientSI2SO',
            storeId: 'deletedcoefficientsi2sostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.coefficientsi2so.DeletedCoefficientSI2SO',
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
            items: [{
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'CoefficientSI2SO').value('DemandCode'),
                dataIndex: 'DemandCode'
            }, {
                text: l10n.ns('tpm', 'CoefficientSI2SO').value('BrandTechName'),
                dataIndex: 'BrandTechName',
                filter: {
                    type: 'search',
                    selectorWidget: 'brandtech',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.brandtech.BrandTech',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'CoefficientSI2SO').value('BrandTechBrandTech_code'),
                dataIndex: 'BrandTechBrandTech_code'
            }, {
                text: l10n.ns('tpm', 'CoefficientSI2SO').value('CoefficientValue'),
                dataIndex: 'CoefficientValue'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.coefficientsi2so.DeletedCoefficientSI2SO',
        items: []
    }]
});
