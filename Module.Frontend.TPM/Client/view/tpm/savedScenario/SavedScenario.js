Ext.define('App.view.tpm.savedScenario.SavedScenario', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.savedScenario',
    title: l10n.ns('tpm', 'compositePanelTitles').value('SavedScenario'),

    //customHeaderItems: [
    //    ResourceMgr.getAdditionalMenu('core').base = {
    //        glyph: 0xf068,
    //        text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

    //        menu: {
    //            xtype: 'customheadermenu',
    //            items: [{
    //                glyph: 0xf4eb,
    //                itemId: 'gridsettings',
    //                text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
    //                action: 'SaveGridSettings',
    //                resource: 'Security'
    //            }]
    //        }
    //    },
    //    ResourceMgr.getAdditionalMenu('core').import = {
    //        glyph: 0xf21b,
    //        text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

    //        menu: {
    //            xtype: 'customheadermenu',
    //            items: [{
    //                glyph: 0xf220,
    //                itemgroup: 'loadimportbutton',
    //                exactlyModelCompare: true,
    //                text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
    //                resource: '{0}',
    //                action: 'FullImportXLSX',
    //                allowFormat: ['zip', 'xlsx']
    //            }, {
    //                glyph: 0xf21d,
    //                itemId: 'loadimporttemplatexlsxbutton',
    //                exactlyModelCompare: true,
    //                text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
    //                action: 'DownloadTemplateXLSX'
    //            }, {
    //                glyph: 0xf21d,
    //                itemId: 'exportxlsxbutton',
    //                exactlyModelCompare: true,
    //                text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
    //                action: 'ExportXLSX'
    //            }]
    //        }
    //    }
    //],

    dockedItems: [],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        name: 'SavedScenarioGrid',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.savedScenario.SavedScenario',
            storeId: 'rsmodestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.savedScenario.SavedScenario',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }
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
                    text: l10n.ns('tpm', 'SavedScenario').value('ScenarioName'),
                    dataIndex: 'ScenarioName'
                },
                {
                    text: l10n.ns('tpm', 'SavedScenario').value('ClientTreeFullPathName'),
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
                },
                {
                    text: l10n.ns('tpm', 'SavedScenario').value('CreateDate'),
                    dataIndex: 'CreateDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
            ]
        }
    },
    {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.savedScenario.SavedScenario',
        items: []
    }
    ]
});
