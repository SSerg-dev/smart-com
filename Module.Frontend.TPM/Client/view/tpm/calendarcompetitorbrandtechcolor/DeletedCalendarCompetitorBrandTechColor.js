Ext.define('App.view.tpm.calendarcompetitorbrandtechcolor.DeletedCalendarCompetitorBrandTechColor', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedcalendarcompetitorbrandtechcolor',
    title: l10n.ns('tpm', 'compositePanelTitles').value('CalendarCompetitorBrandTechColor'),

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
            model: 'App.model.tpm.calendarcompetitorbrandtechcolor.DeletedCalendarCompetitorBrandTechColor',
            storeId: 'deletedcalendarcompetitorbrandtechcolorstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.calendarcompetitorbrandtechcolor.DeletedCalendarCompetitorBrandTechColor',
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
                text: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('Color'),
                dataIndex: 'Color',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div><div style="display:inline-block">{1}</div>', record.get('Color'), record.get('Color'));
                }
            }, {
                text: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('CompanyName'),
                dataIndex: 'CompanyName',
                filter: {
                    type: 'search',
                    selectorWidget: 'companyname',
                    valueField: 'companyName',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
                }, {
                    text: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('BrandTech'),
                    dataIndex: 'BrandTech'
                }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.calendarcompetitorbrandtechcolor.DeletedCalendarCompetitorBrandTechColor',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('Color'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('CompanyName'),
            name: 'CompanyName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('BrandTech'),
            name: 'BrandTech',
        }]
    }]
});
