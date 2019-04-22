Ext.define('App.view.tpm.event.Event', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.event',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Event'),

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
            model: 'App.model.tpm.event.Event',
            storeId: 'eventstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.event.Event',
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
                text: l10n.ns('tpm', 'Event').value('Name'),
                dataIndex: 'Name'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Event').value('Year'),
                dataIndex: 'Year'
            }, {
                text: l10n.ns('tpm', 'Event').value('Period'),
                dataIndex: 'Period',
                filter: {
                    type: 'combo',
                    valueField: 'val',
                    store: {
                        type: 'simplestore',
                        id: 'val',
                        fields: ['val'],
                        data: [{ val: 'P01' }, { val: 'P02' },
                        { val: 'P03' }, { val: 'P04' }, { val: 'P05' },
                        { val: 'P06' }, { val: 'P07' }, { val: 'P08' },
                        { val: 'P09' }, { val: 'P10' }, { val: 'P11' },
                        { val: 'P12' }, { val: 'P13' }]
                    },
                    displayField: 'val',
                    queryMode: 'local',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'Event').value('Description'),
                dataIndex: 'Description'
            },]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.event.Event',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Event').value('Name')
        }, {
            xtype: 'numberfield',
            name: 'Year',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 9999,
            allowBlank: false,
            allowOnlyWhitespace: false,
            fieldLabel: l10n.ns('tpm', 'Event').value('Year')
        }, {
            xtype: 'textfield',
            name: 'Period',
            fieldLabel: l10n.ns('tpm', 'Event').value('Period')
        }, {
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('tpm', 'Event').value('Description')
        }]
    }]
});

