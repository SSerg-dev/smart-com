Ext.define('App.view.tpm.promostatus.PromoStatus', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promostatus',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoStatus'),

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
            model: 'App.model.tpm.promostatus.PromoStatus',
            storeId: 'promostatusstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promostatus.PromoStatus',
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
                text: l10n.ns('tpm', 'PromoStatus').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'PromoStatus').value('SystemName'),
                dataIndex: 'SystemName'
            }, {
                text: l10n.ns('tpm', 'PromoStatus').value('Color'),
                dataIndex: 'Color',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    //var descr = record.get('Color');
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div>', record.get('Color'));
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promostatus.PromoStatus',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Name')
        }, {
            xtype: 'textfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('SystemName')
        }, {
        xtype: 'circlecolorfield',
        name: 'Color',
fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Color'),
}]
    }]
});
