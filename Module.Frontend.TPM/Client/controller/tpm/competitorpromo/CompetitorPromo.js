Ext.define('App.controller.tpm.competitorpromo.CompetitorPromo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'competitorpromo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'competitorpromo directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'competitorpromo #datatable': {
                    activate: this.onActivateCard
                },
                'competitorpromo #detailform': {
                    activate: this.onActivateCard
                },
                'competitorpromo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'competitorpromo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'competitorpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'competitorpromo #table': {
                    click: this.onTableButtonClick
                },
                'competitorpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'competitorpromo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'competitorpromo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'competitorpromo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'competitorpromo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'competitorpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'competitorpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'competitorpromo #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'competitorpromo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'competitorpromo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'competitorpromo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'competitorpromo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'competitorpromo #newImportXLSX': {
                    click: this.onNewImportButtonClick
                },
                'competitorpromoeditor [name=CompetitorBrandTechId]': {
                    change: this.generateCompetitorPromoName
                },
                'competitorpromoeditor [name=MechanicType]': {
                    change: this.generateCompetitorPromoName
                },
                'competitorpromoeditor [name=Discount]': {
                    change: this.generateCompetitorPromoName
                }
            }
        });
    },
    generateCompetitorPromoName: function (button) {
        var editor = button.up('competitorpromoeditor');
        var competitorBrandTechId = editor.down('[name=CompetitorBrandTechId]').rawValue;
        var mechanicTypeCombo = editor.down('[name=MechanicType]').value;
        var discount = editor.down('[name=Discount]').value;
        var handledDiscount = '';//при первой загрузке editor'а - discount равен null
        if (discount != 0 && discount != null) {//проверка дробных частей, если discount равен 0 (',00'), то убирать нули после запятой
            var handledDiscount = discount.toString().includes(",") == true ? discount.split('.') : discount + '%';//к discont прибавляется знак процента
            if (handledDiscount.lenght == 1 || handledDiscount[1] == '00') {
                discount = handledDiscount + '%';
            }
        }
        editor.down('[name=Name]').setValue(competitorBrandTechId + ' ' + mechanicTypeCombo + ' ' + handledDiscount);
    },
    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();
            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
        }
    },

    onNewImportButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            viewClassName = App.Util.buildViewClassName(panel, panel.getBaseModel(), 'Import', 'ParamForm'),
            defaultResource = this.getDefaultResource(button),
            resource = Ext.String.format(button.resource || defaultResource, defaultResource),
            action = Ext.String.format(button.action, resource);;
        Ext.Msg.show({
            title: 'Information!',
            msg: 'All Competitor promo data will be replaced. Confirm?',
            buttons: Ext.Msg.YESNO,
            closable: false,
            buttonText: {
                yes: 'Yes',
                no: 'No'
            },
            width: 300,
            multiline: false,
            fn: function (buttonValue, inputText, showConfig) {
                if (buttonValue === 'yes') {
                    var editor = Ext.create('App.view.core.common.UploadFileWindow', {
                        title: l10n.ns('core').value('uploadFileWindowTitle'),
                        parentGrid: grid,
                        resource: resource,
                        action: action
                    });

                    if (button.additionParameters) {
                        var fields = [];
                        for (var param in button.additionParameters) {
                            if (button.hasOwnProperty(param)) {
                                fields.push({
                                    xtype: 'hiddenfield',
                                    name: param,
                                    value: button.additionParameters[param]
                                });
                            }
                        }
                        editor.down('editorform').add(fields);
                    }
                    var btnBrowse = editor.down('filefield');
                    if (btnBrowse) {
                        var allowFormat = button.allowFormat || ['csv', 'zip'];
                        btnBrowse.allowFormat = allowFormat;
                        btnBrowse.vtypeText = 'Формат файла не поддерживается. Необходим файл формата: ' + allowFormat.join(',');
                    }

                    if (Ext.ClassManager.get(viewClassName)) {
                        var paramForm = Ext.create(viewClassName);
                        var fieldValues = button.fieldValues ? Ext.clone(button.fieldValues) : null;
                        paramForm.initFields(fieldValues);
                        editor.down('#importform').insert(0, paramForm);
                    }
                    editor.show();
                }
            },
            icon: Ext.Msg.INFO
        })
    },
});
