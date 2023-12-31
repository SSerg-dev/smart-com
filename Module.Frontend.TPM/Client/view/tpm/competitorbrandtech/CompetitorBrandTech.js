﻿Ext.define('App.view.tpm.competitorbrandtech.CompetitorBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.competitorbrandtech',
    title: l10n.ns('tpm', 'compositePanelTitles').value('CompetitorBrandTech'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],
    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').base = {
            glyph: 0xf068,
            text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf4eb,
                    itemId: 'gridsettings',
                    text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
                    action: 'SaveGridSettings',
                    resource: 'Security'
                }]
            }
        },
        ResourceMgr.getAdditionalMenu('core').import = {
            glyph: 0xf21b,
            text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf220,
                    itemgroup: 'loadimportbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                    resource: '{0}',
                    action: 'FullImportXLSX',
                    allowFormat: ['zip', 'xlsx']
                }, {
                    glyph: 0xf21d,
                    itemId: 'loadimporttemplatexlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                    action: 'DownloadTemplateXLSX'
                }, {
                    glyph: 0xf21d,
                    itemId: 'exportxlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX'
                }]
            }
        }
    ],
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
            storeId: 'competitorbrandtechstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
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
            items: [{
                text: l10n.ns('tpm', 'CompetitorBrandTech').value('Color'),
                dataIndex: 'Color',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div><div style="display:inline-block">{1}</div>', record.get('Color'), record.get('Color'));
                }
            }, {
                text: l10n.ns('tpm', 'CompetitorBrandTech').value('CompetitorName'),
                dataIndex: 'CompetitorName',
                filter: {
                    type: 'search',
                    selectorWidget: 'competitor',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.competitor.Competitor',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.competitor.Competitor',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
                }, {
                text: l10n.ns('tpm', 'CompetitorBrandTech').value('BrandTech'),
                dataIndex: 'BrandTech',
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
        afterFormShow: function () {
            this.down('circlecolorfield').fireEvent("afterrender");
        },
        items: [{
            xtype: 'circlecolorfield',
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('Color'),
        }, {
            text: l10n.ns('tpm', 'CompetitorBrandTech').value('CompetitorName'),
            dataIndex: 'CompetitorName',
            filter: {
                type: 'search',
                selectorWidget: 'competitor',
                valueField: 'Name',
                store: {
                    type: 'directorystore',
                    model: 'App.model.tpm.competitor.Competitor',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.competitor.Competitor',
                            modelId: 'efselectionmodel'
                        }, {
                            xclass: 'App.ExtTextFilterModel',
                            modelId: 'eftextmodel'
                        }]
                    }
                }
            }
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('BrandTech'),
            name: 'BrandTech'
        }]
    }]
});
