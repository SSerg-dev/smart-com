Ext.define('App.controller.tpm.promoproductcorrection.PromoProductCorrection', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoproductcorrection[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoproductcorrection directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoproductcorrection #datatable': {
                    activate: this.onActivateCard
                },
                'promoproductcorrection #detailform': {
                    activate: this.onActivateCard
                },
                'promoproductcorrection #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoproductcorrection #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoproductcorrection #detail': {
                    click: this.onDetailButtonClick
                },
                'promoproductcorrection #table': {
                    click: this.onTableButtonClick
                },
                'promoproductcorrection #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoproductcorrection #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoproductcorrection #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoproductcorrection #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoproductcorrection #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoproductcorrection #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoproductcorrection #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoproductcorrection #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoproductcorrection #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoproductcorrection #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoproductcorrection #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoproductcorrection #loadimporttemplatexlsxbuttonTLC': {
                    click: this.onLoadImportTemplateXLSXTLCButtonClick
                },
                'promoproductcorrection #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
     
    onCreateButtonClick: function () {

        this.callParent(arguments);

        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
        var createDate = promoproductcorrectioneditor.down('[name=CreateDate]');
        var changeDate = promoproductcorrectioneditor.down('[name=ChangeDate]');
        var userName = promoproductcorrectioneditor.down('[name=UserName]'); 
        var number = promoproductcorrectioneditor.down('[name=Number]'); 
        userName.setValue(App.UserInfo.getUserName());
        number.isCreate = true;


        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);   // приведение к московской timezone
        createDate.setValue(date);                                              // вывести дату в поле 
        changeDate.setValue(date);
    },

     //Получение промо продукта по промо и продукту
    saveModel: function (promoId, productId) {
        if (promoId && productId) {
           
            var parameters = {
                promoId: breeze.DataType.Guid.fmtOData(promoId),
                productId: breeze.DataType.Guid.fmtOData(productId)
            };

            App.Util.makeRequestWithCallback('PromoProducts', 'GetPromoProductByPromoAndProduct', parameters, function (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {

                    if (result.models.length !== 0) {
                        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
                        var promoProductId = promoproductcorrectioneditor.down('[name=PromoProductId]');

                        console.log(result.models.Id);
                        promoProductId.setValue(result.models.Id);
                    } else {
                        App.Notify.pushError('Group is empty');

                    }

                }
                else {
                    App.Notify.pushError(result.message); 
                }
            });
        }
    }

});