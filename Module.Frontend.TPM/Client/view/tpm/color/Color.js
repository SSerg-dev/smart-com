Ext.define('App.view.tpm.color.Color', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.color',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Color'),

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
            model: 'App.model.tpm.color.Color',
            storeId: 'colorstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.color.Color',
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
                text: l10n.ns('tpm', 'Color').value('SystemName'),
                dataIndex: 'DisplayName',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div><div style="display:inline-block">{1}</div>', record.get('SystemName'), record.get('DisplayName'));
                }
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandName'),
                dataIndex: 'BrandName',
                filter: {
                    type: 'search',
                    selectorWidget: 'brand',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.brand.Brand',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.brand.Brand',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'Product').value('TechnologyName'),
                dataIndex: 'TechnologyName',                
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.color.Color',
        afterFormShow: function () {
            // не убирается цвет поля по form.reset
            this.down('circlecolorfield').fireEvent("afterrender");
        },
        items: [{
            xtype: 'circlecolorfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Color').value('SystemName'),          
        }, {
            xtype: 'textfield',
            name: 'DisplayName',
            fieldLabel: l10n.ns('tpm', 'Color').value('DisplayName')
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTechName'),
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            valueField: 'Id',
            displayField: 'BrandName',
            allowBlank: true,
            allowOnlyWhitespace: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brandtech.BrandTech',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'BrandName',
                to: 'BrandName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyName',
        }]
    }]
});
