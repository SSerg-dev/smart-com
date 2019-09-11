Ext.define('App.controller.core.setting.Setting', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'setting[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'setting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'setting #datatable': {
                    activate: this.onActivateCard
                },
                'setting #detailform': {
                    activate: this.onActivateCard
                },
                'setting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'setting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'setting #detail': {
                    click: this.switchToDetailForm
                },
                'setting #table': {
                    click: this.onTableButtonClick
                },
                'setting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'setting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'setting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'setting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'setting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'setting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'setting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'setting #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'setting #exportbutton': {
                    click: this.onExportButtonClick
                },
                'setting #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'setting #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'setting #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'setting #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onGridSelectionChange: function (selModel, selected) {
        this.callParent(arguments);

        var grid = selModel.view.up('grid'),
            form = grid.up('combineddirectorypanel').down('#detailform');

        this.updateValueField(form);
    },

    updateValueField: function (form) {
        var record = form.getRecord(),
            fieldConfig;

        if (record) {
            switch (record.get('Type')) {
                case 'System.DateTime':
                    fieldConfig = {
                        xtype: 'datefield',
                        submitFormat: 'Y-m-d',
                        editable: false
                    };
                    break;
                case 'System.Int32':
                    fieldConfig = {
                        xtype: 'numberfield',
                        allowDecimals: false
                    };
                    break;
                case 'System.Double':
                    fieldConfig = {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    };
                    break;
                case 'System.Boolean':
                    fieldConfig = {
                        xtype: 'booleancombobox',
                        store: {
                            fields: ['id', 'text'],
                            data: [
                                { id: null, text: '\u00a0' },
                                { id: 'true', text: 'true' },
                                { id: 'false', text: 'false' }
                            ]
                        }
                    };
                    break;
                default:
                    fieldConfig = {
                        xtype: 'textfield'
                    };
            }
        }

        if (fieldConfig) {
            var valueField = form.down('[name=Value]'),
                container = valueField.up(),
                index = container.items.indexOf(valueField);

            Ext.applyIf(fieldConfig, {
                name: 'Value',
                fieldLabel: l10n.ns('core', 'Setting').value('Value'),
                ui: 'detail-form-field',
                labelClsExtra: 'singleline-lable',
                allowBlank: true,
                allowOnlyWhitespace: true,
                labelAlign: 'left',
                labelWidth: 170,
                labelSeparator: '',
                labelPad: 0
            });

            container.remove(valueField);
            container.insert(index, fieldConfig);
            form.loadRecord(record);
        }
    }
});