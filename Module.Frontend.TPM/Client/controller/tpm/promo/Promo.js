Ext.define('App.controller.tpm.promo.Promo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onPromoGridSelectionChange,
                    afterrender: this.onGridPromoAfterrender,
                    extfilterchange: this.onExtFilterChange,
                    load: this.onPromoGridStoreLoad
                },
                'promo': {
                    beforedestroy: this.onPromoGridBeforeDestroy
                },
                'promo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promo #createbutton': {
                    click: this.onAllCreateButtonClick
                },
                //'promo #createbutton': {
                //    click: this.onCreateButtonClick
                //},
                //'promo #createinoutbutton': {
                //    click: this.onCreateInOutButtonClick
                //},
                'promo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promo #canchangestateonlybutton': {
                    click: this.onShowEditableButtonClick
                },
                'promotypewindow #ok': {
                    click: this.onPromoTypeOkButtonClick
                },
                'promotypewindow button[itemId!=ok]': {
                    click: this.onSelectionButtonClick
                },
                'promotypewindow': {
                    afterrender: this.onPromoTypeAfterRender
                },
                // promo window
                'promoeditorcustom': {
                    afterrender: this.onPromoEditorCustomAfterrender,
                    beforerender: this.onPromoEditorCustomBeforerender,
                    close: this.onPromoEditorCustomClose,
                },

                // promo main toolbar btn
                'promoeditorcustom #btn_promo': {
                    click: this.onPromoButtonClick
                },
                'promoeditorcustom #btn_support': {
                    click: this.onSupportButtonClick
                },
                'promoeditorcustom #btn_promoBudgets': {
                    click: this.onPromoBudgetsButtonClick
                },
                'promoeditorcustom #btn_promoActivity': {
                    click: this.onPromoActivityButtonClick
                },
                'promoeditorcustom #btn_changes': {
                    click: this.onChangesButtonClick
                },
                'promoeditorcustom #btn_summary': {
                    click: this.onSummaryButtonClick
                },

                // promo basic steps
                'promoeditorcustom #btn_promo_step1': {
                    click: this.onPromoButtonStep1Click
                },
                'promoeditorcustom #btn_promo_step2': {
                    click: this.onPromoButtonStep2Click
                },
                'promoeditorcustom #btn_promo_step3': {
                    click: this.onPromoButtonStep3Click
                },
                'promoeditorcustom #btn_promo_step4': {
                    click: this.onPromoButtonStep4Click
                },
                'promoeditorcustom #btn_promo_step5': {
                    click: this.onPromoButtonStep5Click
                },
                'promoeditorcustom #btn_promo_step6': {
                    click: this.onPromoButtonStep6Click
                },
                'promoeditorcustom #btn_promo_step7': {
                    click: this.onPromoButtonStep7Click
                },
                'promoeditorcustom #btn_promo_step8': {
                    click: this.onPromoButtonStep8Click
                },

                // support steps
                'promoeditorcustom #btn_support_step1': {
                    click: this.onSupportButtonStep1Click
                },
                'promoeditorcustom #btn_support_step2': {
                    click: this.onSupportButtonStep2Click
                },
                'promoeditorcustom #btn_support_step3': {
                    click: this.onSupportButtonStep3Click
                },

                // promo budgets steps
                'promoeditorcustom #btn_promoBudgets_step1': {
                    click: this.onPromoBudgetsButtonStep1Click
                },
                'promoeditorcustom #btn_promoBudgets_step2': {
                    click: this.onPromoBudgetsButtonStep2Click
                },
                'promoeditorcustom #btn_promoBudgets_step3': {
                    click: this.onPromoBudgetsButtonStep3Click
                },
                'promoeditorcustom #btn_promoBudgets_step4': {
                    click: this.onPromoBudgetsButtonStep4Click
                },

                // promo activity steps
                'promoeditorcustom #btn_promoActivity_step1': {
                    click: this.onPromoActivityButtonStep1Click
                },
                'promoeditorcustom #btn_promoActivity_step2': {
                    click: this.onPromoActivityButtonStep2Click
                },
                'promoeditorcustom #btn_splitpublish': {
                    click: this.onSplitAndPublishButtonClick
                },
                'promoeditorcustom #savePromo': {
                    click: this.onSavePromoButtonClick
                },
                'promoeditorcustom #saveAndClosePromo': {
                    click: this.onSaveAndClosePromoButtonClick
                },
                'promoeditorcustom #closePromo': {
                    click: this.onClosePromoButtonClick
                },
                'promoeditorcustom #cancelPromo': {
                    click: this.onCancelPromoButtonClick
                },
                'promoeditorcustom #changePromo': {
                    click: this.onChangePromoButtonClick
                },

                // CustomTopToolbar
                'promoeditorcustom #btn_history': {
                    click: this.onPromoHistoryButtonClick
                },
                'promoeditorcustom #btn_recalculatePromo': {
                    click: this.recalculatePromo
                },
                // import/export
                'promo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                // promo budgets
                'promobudgets': {
                    afterrender: this.onPromoBudgetsAfterRender
                },
                'promobudgets #promoBudgets_step1 numberfield': {
                    change: this.onPromoBudgetsStep1Change
                },

                // promo calculation                
                'promomechanic numberfield[name=MarsMechanicDiscount]': {
                    change: this.onMarsMechanicDiscountChange
                },
                'promocalculation #calculations_step2 numberfield[name=PlanPromoIncrementalLSV]': {
                    change: this.onPlanPromoIncrementalLSVChange
                },
                'promocalculation #calculations_step2 numberfield': {
                    change: this.onCalculationPlanChange
                },
                'promocalculation #calculations_step3 numberfield': {
                    change: this.onCalculationPlanChange
                },

                // promo activity
                'promoactivity': {
                    afterrender: this.onPromoActivityAfterRender
                },

                'promoactivity #promoActivity_step2 #activityUploadPromoProducts': {
                    click: this.onActivityUploadPromoProductsClick
                },

                // методы решают проблему с "Настроить таблицу"
                'promo customheadermenu #gridsettings': {
                    click: this.onGridSettingsClick
                },
                'gridsettingswindow #save': {
                    click: this.onGridSettingsSaveClick
                },
                'gridsettingswindow': {
                    close: this.onGridSettingsWindowClose
                },

                'promoeditorcustom #btn_showlog': {
                    click: this.onShowLogButtonClick
                },
                // Promo Budgets Details Window
                'promobudgetsdetailswindow': {
                    afterrender: this.onPromoBudgetsDetailsWindowAfterRender
                },
                // Promo Finance Details Window
                'promofinancedetailswindow': {
                    afterrender: this.onPromoFinanceDetailsWindowAfterRender
                },
                // Promo Activity Details Window
                'promoactivitydetailswindow': {
                    afterrender: this.onPromoActivityDetailsWindowAfterRender
                },
                // Promo Product Subrange Details Window
                'promoproductsubrangedetailswindow': {
                    afterrender: this.onPromoProductSubrangeDetailsWindowAfterRender
                },
                'promoeditorcustom #activityDetailButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #budgetDetailButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #financeDetailButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #brandTechButton': {
                    click: this.onSummaryDetailButtonClick
                },
                'promoeditorcustom #basicPromoPanel': {
                    afterrender: this.onBasicPanelAfterrender
                },

                'promoeditorcustom [name=GrowthAccelerationCheckbox]': {
                    change: this.onGrowthAccelerationCheckboxChange
                },
                'promoeditorcustom [name=ApolloExportCheckbox]': {
                    change: this.onApolloExportCheckboxChange
                },

                // choose client
                'promoclient #promoClientSettignsBtn': {
                    click: this.onPromoClientSettignsClick
                },
                'promoclient #choosePromoClientBtn': {
                    click: this.onChoosePromoClientClick
                },
                'promoclientchoosewindow clienttree basetreegrid': {
                    checkchange: this.onClientTreeCheckChange
                },
                'promoclientchoosewindow #choose': {
                    click: this.onChooseClientTreeClick
                },
                'promoclientchoosewindow #dateFilter': {
                    click: this.onClientDateFilterButtonClick
                },

                // choose product
                'promobasicproducts #choosePromoProductsBtn': {
                    click: this.onChoosePromoProductsBtnClick
                },
                'promobasicproducts #promoBasicProducts_FilteredList': {
                    afterrender: this.onFilteredListBtnAfterRender,
                    click: this.onFilteredListBtnClick
                },
                'promobasicproducts #promoBasicProducts_ProductList': {
                    click: this.onProductListBtnClick
                },
                'promoproductchoosewindow producttree basetreegrid': {
                    checkchange: this.onProductTreeCheckChange
                },
                'promoproductchoosewindow #choose': {
                    click: this.onChooseProductTreeClick
                },
                'promoproductchoosewindow #dateFilter': {
                    click: this.onProductDateFilterButtonClick
                },
            }
        });
    },

    onGridPromoAfterrender: function (grid) {
        var store = grid.getStore();

        store.on('load', function (store) {
            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var view = grid.getView();

                if (view && view.positionY) {
                    var jspData = $(view.getEl().dom).data('jsp');

                    jspData.scrollToY(view.positionY, false);
                    view.positionY = 0;
                }
            }
            var promoController = App.app.getController('tpm.promo.Promo');
            promoController.massApprovalButtonDisable(grid, store);
        });

        store.on('beforeload', function (store) {
            var maButton = grid.up().down('custombigtoolbar').down('#massapprovalbutton');
            maButton.setDisabled(true);
        });

        this.onGridAfterrender(grid);
    },

    massApprovalButtonDisable: function (grid, store) {
        var promoHelperController = App.app.getController('tpm.promo.PromoHelper');
        var filter = store.fixedFilters ? store.fixedFilters['hiddenExtendedFilter'] : null;

        var onApprovalFilterDP = promoHelperController.getOnApprovalFilterDP();
        var onApprovalFilterDF = promoHelperController.getOnApprovalFilterDF();
        var onApprovalFilterCMM = promoHelperController.getOnApprovalFilterCMM();
        var maButton = grid.up().down('custombigtoolbar').down('#massapprovalbutton');

        var isDisabled = !this.compareFilters(filter, onApprovalFilterDP) &&
            !this.compareFilters(filter, onApprovalFilterDF) &&
            !this.compareFilters(filter, onApprovalFilterCMM);

        maButton.setDisabled(isDisabled);
    },   

    compareFilters: function (filter1, filter2) {
        var isSame = true;
        if (filter1 && filter2) {
            isSame = filter1.rules.length == filter2.rules.length;
            if (isSame) {

                var filterProperties1 = this.getFilterProperties(filter1);
                var filterProperties2 = this.getFilterProperties(filter2);

                var isSame =
                    (filterProperties1.operators.length == filterProperties2.operators.length) &&
                    (filterProperties1.properties.length == filterProperties2.properties.length) &&
                    (filterProperties1.values.length == filterProperties2.values.length) &&
                    filterProperties1.operators.every(function (element, index) {
                        return element === filterProperties2.operators[index];
                    }) &&
                    filterProperties1.properties.every(function (element, index) {
                        return element === filterProperties2.properties[index];
                    }) &&
                    filterProperties1.values.every(function (element, index) {
                        return element === filterProperties2.values[index];
                    });
            }
        } else {
            isSame = false;
        }
        return isSame;
    },

    getFilterProperties: function (filter) {
        var filterProperties = {
            operators: [],
            properties: [],
            values: []
        };
        filterProperties.operators.push(filter.operator);

        filter.rules.forEach(function (item) {
            if (!item.rules) {
                var date = new Date(Date.parse(item.value));

                if (!isNaN(date)) {
                    date.setMinutes(0, 0, 0);

                    filterProperties.values.push(date.getTime());
                    filterProperties.properties.push(item.property);
                } else {
                    filterProperties.values.push(item.value);
                    filterProperties.properties.push(item.property);
                }
            } else {
                var promoController = App.app.getController('tpm.promo.Promo');
                var nestedFilter = promoController.getFilterProperties(item);
                Array.prototype.push.apply(filterProperties.operators, nestedFilter.operators);
                Array.prototype.push.apply(filterProperties.properties, nestedFilter.properties);
                Array.prototype.push.apply(filterProperties.values, nestedFilter.values);
            }
        });

        return filterProperties;
    },

    onBasicPanelAfterrender: function (component, eOpts) {
        $(component.getTargetEl().dom).on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
            var panel = component.up();
            if (panel) { // TODO: отрефакторить?

                var btnStep1 = panel.down('#btn_promo_step1');
                var btnStep2 = panel.down('#btn_promo_step2');
                var btnStep3 = panel.down('#btn_promo_step3');
                var btnStep4 = panel.down('#btn_promo_step4');
                var btnStep5 = panel.down('#btn_promo_step5');
                var btnStep6 = panel.down('#btn_promo_step6');
                var btnStep7 = panel.down('#btn_promo_step7');
                var btnStep8 = panel.down('#btn_promo_step8');

                var formStep1 = panel.down('#promo_step1');
                var formStep2 = panel.down('#promo_step2');
                var formStep3 = panel.down('#promo_step3');
                var formStep4 = panel.down('#promo_step4');
                var formStep5 = panel.down('#promo_step5');
                var formStep6 = panel.down('#promo_step6');
                var formStep7 = panel.down('#promo_step7');
                var formStep8 = panel.down('#promo_step8');

                if (formStep8.needToSetHeight && isAtTop) {
                    formStep8.setHeight(panel.getHeight() - 20);
                    formStep8.needToSetHeight = false;
                } else {
                    component._refreshScroll(component);
                }

                var h1 = formStep1.height;
                var h1_2 = h1 + formStep2.height;
                var h1_2_3 = h1_2 + formStep3.height;
                var h1_2_3_4 = h1_2_3 + formStep4.height;
                var h1_2_3_4_5 = h1_2_3_4 + formStep5.height;
                var h1_2_3_4_5_6 = h1_2_3_4_5 + formStep6.height;
                var h1_2_3_4_5_6_7 = h1_2_3_4_5_6 + formStep7.height;
                var h1_2_3_4_5_6_7_8 = h1_2_3_4_5_6_7 + formStep8.height;

                var _deltaY = scrollPositionY + 100;

                // Step 1 - client
                if (_deltaY <= h1) {
                    btnStep1.addClass('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.addClass('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');

                    // Step 2 - product
                } else if (_deltaY > h1 && _deltaY <= h1_2) {
                    btnStep1.removeCls('selected');
                    btnStep2.addClass('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.addClass('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 3 - mechanic
                } else if (_deltaY > h1_2 && _deltaY <= h1_2_3) {
                    event.preventDefault();

                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.addClass('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.addClass('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 4 - period
                } else if (_deltaY > h1_2_3 && _deltaY <= h1_2_3_4) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.addClass('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.addClass('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');


                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 5 - budget year
                } else if (_deltaY > h1_2_3_4 && _deltaY <= h1_2_3_4_5) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.addClass('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.addClass('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);

                    // Step 6 - settings
                } else if (_deltaY > h1_2_3_4_5 && _deltaY <= h1_2_3_4_5_6) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.addClass('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.addClass('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);
                    // Step 6 - settings
                } else if (_deltaY > h1_2_3_4_5_6 && _deltaY <= h1_2_3_4_5_6_7) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.addClass('selected');
                    btnStep8.removeCls('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.addClass('promo-header-item-active');
                    formStep8.header.removeCls('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);
                } else if (_deltaY > h1_2_3_4_5_6_7 && _deltaY <= h1_2_3_4_5_6_7_8) {
                    btnStep1.removeCls('selected');
                    btnStep2.removeCls('selected');
                    btnStep3.removeCls('selected');
                    btnStep4.removeCls('selected');
                    btnStep5.removeCls('selected');
                    btnStep6.removeCls('selected');
                    btnStep7.removeCls('selected');
                    btnStep8.addClass('selected');

                    formStep1.header.removeCls('promo-header-item-active');
                    formStep2.header.removeCls('promo-header-item-active');
                    formStep3.header.removeCls('promo-header-item-active');
                    formStep4.header.removeCls('promo-header-item-active');
                    formStep5.header.removeCls('promo-header-item-active');
                    formStep6.header.removeCls('promo-header-item-active');
                    formStep7.header.removeCls('promo-header-item-active');
                    formStep8.header.addClass('promo-header-item-active');

                    var mousedownEvent = document.createEvent('MouseEvents');
                    mousedownEvent.initEvent('mousedown', true, true)
                    panel.el.dom.dispatchEvent(mousedownEvent);
                } else {
                    console.log('undefined step');
                }
            }
        });

        var mainTab = component.up('promoeditorcustom').down('button[itemId=btn_promo]');
        var stepButtons = component.up('panel[name=promo]').down('custompromotoolbar');
        checkMainTab(stepButtons, mainTab);
    },


    // открытие окон просмотра детальной информации из дашборда
    onSummaryDetailButtonClick: function (button) {
        Ext.widget(button.windowName).show();
    },

    onPromoEditorCustomAfterrender: function (promoEditorCustom) {
        if (promoEditorCustom.isInOutPromo || (promoEditorCustom.model && promoEditorCustom.model.data.InOut)
            || (promoEditorCustom.assignedRecord && promoEditorCustom.assignedRecord.data.InOut)) {


        }
        //теперь каждое промо имеет глиф и название
        promoEditorCustom.query('#btn_promoInOut')[0].show();
        promoEditorCustom.query('#btn_promoInOut')[0].setGlyph(parseInt('0x' + promoEditorCustom.promotypeGlyph, 16));
        promoEditorCustom.query('#btn_promoInOut')[0].setText(promoEditorCustom.promotypeName);
        this.hideEditButtonForSomeRole();
    },

    onPromoEditorCustomBeforerender: function () {
        this.setCustomTips();
    },

    setCustomTips: function () {
        var elementsWithTips = Ext.ComponentQuery.query('*[customTip*=]');

        elementsWithTips.forEach(function (el) {
            var me = el;

            // Перезапись стандартного создания ToolTip
            Ext.override(me, {
                afterFirstLayout: function () {
                    var me = this;
                    this.callParent(arguments);

                    me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                        target: me.labelEl,
                        preventLoseFocus: true,
                        trackMouse: true,
                        html: me.customTip
                    });
                }
            });
        })
    },

    onPromoEditorCustomClose: function () {
        var toolbar = Ext.ComponentQuery.query('readonlydirectorytoolbar');
        if (toolbar.length > 0)
            toolbar[0].setDisabled(false);

        // закрываем подключение к хабу
        requestHub($.connection.logHub.server.disconnectFromHub);
    },

    // =============== Steps ===============

    // promo main toolbar btn
    onPromoButtonClick: function (button) {
        this.setButtonsState(button);
    },

    onSupportButtonClick: function (button) {
        this.setButtonsState(button);
    },

    onPromoBudgetsButtonClick: function (button) {
        this.setButtonsState(button);
    },

    onPromoActivityButtonClick: function (button) {
        this.setButtonsState(button);
    },
    onSummaryButtonClick: function (button, e) {
        var me = this,
            promoeditorcustom = button.up('promoeditorcustom'),
            mask = promoeditorcustom.setLoading(true);
        // для того чтобы маска отрисовалась первой
        Ext.Function.defer(me.fillSummaryPanel, 1, me, [button, mask]);
    },

    fillSummaryPanel: function (button, mask) {
        var me = this,
            window = button.up('window'),
            record = me.getRecord(window),
            promoEditorCustom = button.up('promoeditorcustom'),
            isInOut = (promoEditorCustom.isInOutPromo || (promoEditorCustom.model && promoEditorCustom.model.data.InOut));
        var summary = window.down('panel[name=summary]');

        if (!isInOut) {
            summary.down('[name=UpliftFieldset]').show();
        }

        me.updateSummaryInformationPanel(summary, record);

        me.updateSummaryPlanFactLabels(summary, record, isInOut);

        me.setButtonsState(button);

        var budgetsFieldset = summary.down('panel[name=budgetsFieldset]'),
            activityFieldset = summary.down('panel[name=activityFieldset]'),
            roiFieldset = summary.down('panel[name=roiFieldset]');
        // activity Data
        var activityChartName, activityChartFields, ChartData;
        if (isInOut) {
            activityChartName = 'inoutpromoactivitychart';
            activityChartFields = ['name', 'Inc'];
        }
        else {
            activityChartName = 'promoactivitychart';
            activityChartFields = ['name', 'Inc', 'BL'];
        }

        var factInc = record.get('ActualPromoIncrementalLSV') || 0,
            planInc = record.get('PlanPromoIncrementalLSV') || 0;
        if (isInOut) {
            var ChartData = [
                { name: 'Plan', Inc: planInc },
                { name: 'Actual', Inc: factInc },
            ]
        } else {
            var factBL = record.get('ActualPromoBaselineLSV') || 0,
                planBL = record.get('PlanPromoBaselineLSV') || 0;

            if (factBL === -factInc) {
                factBL = 0;
                factInc = 0;
            }
            if (planBL === -planInc) {
                planBL = 0;
                planInc = 0;
            }

            if (!(factBL === 0 && factInc === 0)) {
                factInc = (factInc / (factBL + factInc)) * 100;
                factInc = Ext.util.Format.round(factInc, 2);
                factBL = 100 - factInc;
            }
            if (!(planBL === 0 && planInc === 0)) {
                planInc = (planInc / (planBL + planInc)) * 100;
                planInc = Ext.util.Format.round(planInc, 2);
                planBL = 100 - planInc;
            }

            var ChartData = [
                { name: 'Plan', BL: planBL, Inc: planInc },
                { name: 'Actual', BL: factBL, Inc: factInc },
            ]
        }
        // ROI Data
        var planROI = record.get('PlanPromoNetROIPercent') || 0,
            factROI = record.get('ActualPromoNetROIPercent') || 0;
        planROI = Ext.util.Format.round(planROI, 2);
        factROI = Ext.util.Format.round(factROI, 2);
        var maximum = planROI > factROI ? planROI : factROI;
        maximum = maximum == 0 ? 100 : Math.ceil(maximum / 100) * 100;
        var ROIChartData = [
            { name: l10n.ns('tpm', 'PromoSummary').value('Plan'), value: planROI },
            { name: l10n.ns('tpm', 'PromoSummary').value('Actual'), value: factROI },
        ];

        // budget Data
        var Cost = record.get('ActualPromoCost') || 0;

        if (Cost > 0) {
            var Shopper = record.get('ActualPromoTIShopper') || 0,
                CostProd = record.get('ActualPromoCostProduction') || 0,
                Marketing = record.get('ActualPromoTIMarketing') || 0,
                Branding = record.get('ActualPromoBranding') || 0,
                BTL = record.get('ActualPromoBTL') || 0,
                PlanOrActual = 'Actual';
        } else {
            var Shopper = record.get('PlanPromoTIShopper') || 0,
                Cost = record.get('PlanPromoCost') || 0,
                CostProd = record.get('PlanPromoCostProduction') || 0,
                Marketing = record.get('PlanPromoTIMarketing') || 0,
                Branding = record.get('PlanPromoBranding') || 0,
                BTL = record.get('PlanPromoBTL') || 0,
                PlanOrActual = 'Plan';
        }

        budgetData = [
            { name: l10n.ns('tpm', 'PromoSummary').value('MarketingTI'), value: Marketing },
            { name: l10n.ns('tpm', 'PromoSummary').value('ShopperTI'), value: Shopper },
            { name: l10n.ns('tpm', 'PromoSummary').value('BTL'), value: BTL },
            { name: l10n.ns('tpm', 'PromoSummary').value('CostProduction'), value: CostProd },
            { name: l10n.ns('tpm', 'PromoSummary').value('Branding'), value: Branding }
        ]
        // Если графики уже добавлены в дашборд, просто перезагружаем их сторы
        if (activityFieldset.items.length != 0) {
            var activityChartStore = activityFieldset.down(activityChartName).getStore(),
                budgetsChart = budgetsFieldset.down('promobudgetschart'),
                budgetsChartStore = budgetsChart.getStore(),
                roiChart = roiFieldset.down('promoroichart'),
                roiChartStore = roiChart.getStore();

            activityChartStore.loadData(ChartData);

            roiChart.maximum = maximum;
            roiChart.fireEvent('beforerender', roiChart);
            roiChartStore.loadData(ROIChartData);
            roiChart.fireEvent('resize', roiChart);

            budgetsChart.cost = Cost;
            budgetsChart.planOrActual = PlanOrActual;
            budgetsChart.fireEvent('resize', budgetsChart);
            budgetsChartStore.loadData(budgetData);
        } else {
            // Добавляем графики в дашборд, если их ещё нет там
            activityFieldset.add({
                width: 500,
                height: 300,
                xtype: activityChartName,
                store: Ext.create('Ext.data.Store', {
                    storeId: 'planpromoactivitystore',
                    fields: activityChartFields,
                    data: ChartData
                })
            });

            budgetsFieldset.add({
                xtype: 'promobudgetschart',
                store: Ext.create('Ext.data.Store', {
                    storeId: 'promobudgetsstore',
                    fields: ['name', 'value'],
                    data: budgetData
                }),
                cost: Cost,
                planOrActual: PlanOrActual,
            });

            roiFieldset.add([{
                xtype: 'promoroichart',
                store: Ext.create('Ext.data.Store', {
                    storeId: 'promofinancechartstore',
                    fields: ['name', 'value'],
                    data: ROIChartData
                }),
                maximum: maximum
            }]);
        }
        mask.destroy();
    },

    // Обновление данных на первой панели дашборда
    updateSummaryInformationPanel: function (window, record) {
        var promoInformationPanel = window.down('container[name=promoInformationPanel]'),
            promoNameLabel = promoInformationPanel.down('label[name=promoNameLabel]'),
            clientLabel = promoInformationPanel.down('label[name=clientLabel]'),
            summaryStatusField = promoInformationPanel.down('fieldset[name=summaryStatusField]'),
            marsMechanicLabel = promoInformationPanel.down('label[name=marsMechLabel]'),
            instoreMechanicLabel = promoInformationPanel.down('label[name=instoreMechLabel]'),
            durationDatesLabel = promoInformationPanel.down('label[name=durationDateLabel]'),
            dispatchesDatesLabel = promoInformationPanel.down('label[name=dispatchDateLabel]');

        var promoName = Ext.String.format('{0} (ID: {1})', record.get('Name'), record.get('Number'));
        promoNameLabel.setText(promoName);

        var client = record.get('PromoClientName');
        clientLabel.setText(client)

        summaryStatusField.update({ StatusColor: record.get('PromoStatusColor'), StatusName: record.get('PromoStatusName') });

        instoreMechanicLabel.setText(record.get('MechanicIA'));
        marsMechanicLabel.setText(record.get('Mechanic'));

        var durationText = Ext.String.format('{0} - {1}', Ext.Date.format(record.get('StartDate'), 'd.m.Y'), Ext.Date.format(record.get('EndDate'), 'd.m.Y'));
        durationDatesLabel.setText(durationText);

        var dispatchDatesText = Ext.String.format('{0} - {1}', Ext.Date.format(record.get('DispatchesStart'), 'd.m.Y'), Ext.Date.format(record.get('DispatchesEnd'), 'd.m.Y'));
        dispatchesDatesLabel.setText(dispatchDatesText);
    },

    // Обновление текстовых данных план/факт 
    updateSummaryPlanFactLabels: function (summary, record, isInOut) {
        var me = this;

        if (!isInOut) {
            var ActualPromoUpliftPercent = record.get('ActualPromoUpliftPercent') || 0,
                PlanPromoUpliftPercent = record.get('PlanPromoUpliftPercent') || 0;
            ActualPromoUpliftPercent = Ext.util.Format.round(ActualPromoUpliftPercent, 2);
            PlanPromoUpliftPercent = Ext.util.Format.round(PlanPromoUpliftPercent, 2);

            var planUpliftLabel = summary.down('fieldset[name=planUpliftLabel]'),
                actualUpliftLabel = summary.down('fieldset[name=actualUpliftLabel]');

            planUpliftLabel.setTitle(PlanPromoUpliftPercent + '%');
            actualUpliftLabel.setTitle(ActualPromoUpliftPercent + '%');
        }

        var ActualPromoIncrementalNSV = record.get('ActualPromoNetIncrementalNSV') || 0,
            PlanPromoIncrementalNSV = record.get('PlanPromoNetIncrementalNSV') || 0,
            PercentPromoIncrementalNSV = 0,

            ActualPromoIncrementalLSV = record.get('ActualPromoNetIncrementalLSV') || 0,
            PlanPromoIncrementalLSV = record.get('PlanPromoNetIncrementalLSV') || 0,
            PercentPromoIncrementalLSV = 0,

            ActualPromoNSV = record.get('ActualPromoNetNSV') || 0,
            PlanPromoNSV = record.get('PlanPromoNetNSV') || 0,
            PercentPromoNSV = 0,

            ActualPromoIncrementalEarnings = record.get('ActualPromoNetIncrementalEarnings') || 0,
            PlanPromoIncrementalEarnings = record.get('PlanPromoNetIncrementalEarnings') || 0,
            PercentPromoIncrementalEarnings = 0;

        ActualPromoIncrementalNSV = Ext.util.Format.round(ActualPromoIncrementalNSV / 1000000, 2);
        PlanPromoIncrementalNSV = Ext.util.Format.round(PlanPromoIncrementalNSV / 1000000, 2);
        ActualPromoIncrementalLSV = Ext.util.Format.round(ActualPromoIncrementalLSV / 1000000, 2);
        PlanPromoIncrementalLSV = Ext.util.Format.round(PlanPromoIncrementalLSV / 1000000, 2);
        ActualPromoNSV = Ext.util.Format.round(ActualPromoNSV / 1000000, 2);
        PlanPromoNSV = Ext.util.Format.round(PlanPromoNSV / 1000000, 2);
        ActualPromoIncrementalEarnings = Ext.util.Format.round(ActualPromoIncrementalEarnings / 1000000, 2);
        PlanPromoIncrementalEarnings = Ext.util.Format.round(PlanPromoIncrementalEarnings / 1000000, 2);

        if (!(PlanPromoIncrementalNSV === 0)) {
            PercentPromoIncrementalNSV = (ActualPromoIncrementalNSV / PlanPromoIncrementalNSV) * 100 - 100;
            PercentPromoIncrementalNSV = Ext.util.Format.round(PercentPromoIncrementalNSV, 2);
        }
        if (!(PlanPromoIncrementalLSV === 0)) {
            PercentPromoIncrementalLSV = (ActualPromoIncrementalLSV / PlanPromoIncrementalLSV) * 100 - 100;
            PercentPromoIncrementalLSV = Ext.util.Format.round(PercentPromoIncrementalLSV, 2);
        }
        if (!(PlanPromoNSV === 0)) {
            PercentPromoNSV = (ActualPromoNSV / PlanPromoNSV) * 100 - 100;
            PercentPromoNSV = Ext.util.Format.round(PercentPromoNSV, 2);
        }
        if (!(PlanPromoIncrementalEarnings === 0)) {
            PercentPromoIncrementalEarnings = (ActualPromoIncrementalEarnings / PlanPromoIncrementalEarnings) * 100 - 100;
            PercentPromoIncrementalEarnings = Ext.util.Format.round(PercentPromoIncrementalEarnings, 2);
        }


        var actualIncNSVLabel = summary.down('label[name=actualIncNSVLabel]'),
            planIncNSVLabel = summary.down('label[name=planIncNSVLabel]'),
            percentIncNSVLabel = summary.down('label[name=percentIncNSVLabel]'),

            actualIncLSVLabel = summary.down('label[name=actualIncLSVLabel]'),
            planIncLSVLabel = summary.down('label[name=planIncLSVLabel]'),
            percentIncLSVLabel = summary.down('label[name=percentIncLSVLabel]'),

            actualPromoNSVLabel = summary.down('label[name=actualPromoNSVLabel]'),
            planPromoNSVLabel = summary.down('label[name=planPromoNSVLabel]'),
            percentPromoNSVLabel = summary.down('label[name=percentPromoNSVLabel]'),

            actualEarningsLabel = summary.down('label[name=actualEarningsLabel]'),
            planEarningsLabel = summary.down('label[name=planEarningsLabel]'),
            percentEarningsLabel = summary.down('label[name=percentEarningsLabel]');

        actualIncNSVLabel.setText(ActualPromoIncrementalNSV);
        planIncNSVLabel.setText(PlanPromoIncrementalNSV);
        percentIncNSVLabel.setText(PercentPromoIncrementalNSV + '%');

        actualIncLSVLabel.setText(ActualPromoIncrementalLSV);
        planIncLSVLabel.setText(PlanPromoIncrementalLSV);
        percentIncLSVLabel.setText(PercentPromoIncrementalLSV + '%');

        actualPromoNSVLabel.setText(ActualPromoNSV);
        planPromoNSVLabel.setText(PlanPromoNSV);
        percentPromoNSVLabel.setText(PercentPromoNSV + '%');

        actualEarningsLabel.setText(ActualPromoIncrementalEarnings);
        planEarningsLabel.setText(PlanPromoIncrementalEarnings);
        percentEarningsLabel.setText(PercentPromoIncrementalEarnings + '%');

        (PercentPromoIncrementalNSV < 0) ? percentIncNSVLabel.addCls('redPercent') : percentIncNSVLabel.addCls('greenPercent');
        (PercentPromoIncrementalLSV < 0) ? percentIncLSVLabel.addCls('redPercent') : percentIncLSVLabel.addCls('greenPercent');
        (PercentPromoNSV < 0) ? percentPromoNSVLabel.addCls('redPercent') : percentPromoNSVLabel.addCls('greenPercent');
        (PercentPromoIncrementalEarnings < 0) ? percentEarningsLabel.addCls('redPercent') : percentEarningsLabel.addCls('greenPercent');
    },

    // Получение Цифры с иконкой для фактического значения в сравнении с плановым
    getStyledFactValue: function (planValue, factValue, template) {
        var isNegative = planValue > factValue;
        var isPositive = planValue < factValue;
        var style = 'default',
            icon = '';
        if (isNegative) {
            style = 'negative';
            icon = '<span class="mdi mdi-menu-down negative-result-glyph" style="font-family:MaterialDesignIcons;"></span>'
        } else if (isPositive) {
            style = 'positive';
            icon = '<span class="mdi mdi-menu-up positive-result-glyph" style="font-family:MaterialDesignIcons;"></span>';
        }
        return Ext.String.format(template, style, factValue, icon);
    },

    onChangesButtonClick: function (button) {
        this.setButtonsState(button);
        var window = button.up('window');
        var approvalhistory = window.down('container[name=changes]');
        var panel = approvalhistory.down('panel');
        var parentHeight = approvalhistory.up().getHeight();
        approvalhistory.down('fieldset').setHeight(parentHeight - 13);

        var promoeditorcustom = button.up('promoeditorcustom');
        var promoId = promoeditorcustom.promoId;

        promoeditorcustom.setLoading(true);

        var parameters = {
            promoKey: promoId
        };
        App.Util.makeRequestWithCallback('PromoStatusChanges', 'PromoStatusChangesByPromo', parameters, function (data) {
            var result = Ext.JSON.decode(data.httpResponse.data.value);
            var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
            if (result.success) {
                if (!promoeditorcustom.isDestroyed) {
                    promoeditorcustom.fireEvent('fillWFPanel', panel, result, approvalhistory, promoeditorcustom);
                }
            } else {
                promoeditorcustom.setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
            }
        });
        if (promoId != null) {

        }
    },
    // переключение между вкладками формы промо
    setButtonsState: function (button) {
        var window = button.up('window'),
            itemNamesArray = ['promo', 'promoBudgets', 'promoActivity', 'summary', 'changes'],
            buttonItem = button.itemId.replace('btn_', '');
        itemNamesArray.forEach(function (item) {
            var selectedButton = item == buttonItem,
                query = Ext.String.format('[name={0}]', item),
                curWin = window.down(Ext.String.format('container{0}', query)) || window.down(Ext.String.format('panel{0}', query));
            curWin.setVisible(selectedButton);
            var curBtn = window.down(Ext.String.format('#btn_{0}', item));
            if (selectedButton) {
                curBtn.addClass('selected');
            } else {
                curBtn.removeCls('selected');
            }
        })
    },

    // promo steps
    onPromoButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        jspData.scrollToY(0, true);
        button.addClass('selected');
    },

    onPromoButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promobasicproducts#promo_step2').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep3Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promomechanic[itemId=promo_step3]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep4Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promoperiod[itemId=promo_step4]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep5Click: function (button) {
        var container = button.up('window').down('container[name=promo]');

        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promobudgetyear[itemId=promo_step5]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep6Click: function (button) {
        var container = button.up('window').down('container[name=promo]');
        //   container._refreshScroll(container);
        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promoevent[itemId=promo_step6]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep7Click: function (button) {
        var container = button.up('window').down('container[name=promo]');
        //   container._refreshScroll(container);
        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step8').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promosettings[itemId=promo_step7]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoButtonStep8Click: function (button) {
        var container = button.up('window').down('container[name=promo]');
        //   container._refreshScroll(container);
        container.down('#btn_promo_step1').removeCls('selected');
        container.down('#btn_promo_step2').removeCls('selected');
        container.down('#btn_promo_step3').removeCls('selected');
        container.down('#btn_promo_step4').removeCls('selected');
        container.down('#btn_promo_step5').removeCls('selected');
        container.down('#btn_promo_step6').removeCls('selected');
        container.down('#btn_promo_step7').removeCls('selected');

        var jspData = $(container.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
        var el = $(container.down('promoadjustment[itemId=promo_step8]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // promo budgets steps
    onPromoBudgetsButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step2').removeCls('selected');
        container.down('#btn_promoBudgets_step3').removeCls('selected');
        container.down('#btn_promoBudgets_step4').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        jspData.scrollToY(0, true);
        button.addClass('selected');
    },

    onPromoBudgetsButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step1').removeCls('selected');
        container.down('#btn_promoBudgets_step3').removeCls('selected');
        container.down('#btn_promoBudgets_step4').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoBudgets_step2]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoBudgetsButtonStep3Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step1').removeCls('selected');
        container.down('#btn_promoBudgets_step2').removeCls('selected');
        container.down('#btn_promoBudgets_step4').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoBudgets_step3]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoBudgetsButtonStep4Click: function (button) {
        var container = button.up('window').down('container[name=promoBudgets]')

        container.down('#btn_promoBudgets_step1').removeCls('selected');
        container.down('#btn_promoBudgets_step2').removeCls('selected');
        container.down('#btn_promoBudgets_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=promoBudgetsContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoBudgets_step4]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // promo activity
    onPromoActivityButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=promoActivity]')

        container.down('#btn_promoActivity_step2').removeCls('selected');
        var jspData = $(container.down('panel[name=promoActivityContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoActivity_step1]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onPromoActivityButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=promoActivity]')

        container.down('#btn_promoActivity_step1').removeCls('selected');
        container.down('#btn_promoActivity_step1').setGlyph(0xf133);
        var jspData = $(container.down('panel[name=promoActivityContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=promoActivity_step2]').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // support steps
    onSupportButtonStep1Click: function (button) {
        var container = button.up('window').down('container[name=support]')

        container.down('#btn_support_step2').removeCls('selected');
        container.down('#btn_support_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=supportContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=support_step1] customtoptreetoolbar').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onSupportButtonStep2Click: function (button) {
        var container = button.up('window').down('container[name=support]')

        container.down('#btn_support_step1').removeCls('selected');
        container.down('#btn_support_step3').removeCls('selected');

        var jspData = $(container.down('panel[name=supportContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=support_step2] customtoptreetoolbar').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    onSupportButtonStep3Click: function (button) {
        var container = button.up('window').down('container[name=support]')

        container.down('#btn_support_step1').removeCls('selected');
        container.down('#btn_support_step2').removeCls('selected');

        var jspData = $(container.down('panel[name=supportContainer]').getTargetEl().dom).data('jsp');
        var el = $(container.down('panel[itemId=support_step3] customtoptreetoolbar').getTargetEl().dom);
        jspData.scrollToElement(el, true, true);
        jspData.scrollToElement(el, true, true);
        button.addClass('selected');
    },

    // =============== Promo buttons ===============

    onCreateButtonClick: function (button, e, schedulerData, isInOutPromo, promotype) {

        var me = this;
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        promoeditorcustom.isInOutPromo = isInOutPromo;
        promoeditorcustom.promotypeId = promotype.Id;
        promoeditorcustom.promotypeName = promotype.Name;
        promoeditorcustom.promotypeGlyph = promotype.Glyph;
        promoeditorcustom.promotypeSystemName = promotype.SystemName;
        this.setPromoType(promotype.Name, promoeditorcustom);
        promoeditorcustom.isCreating = true;
        // из-за вызова из календаря, нужно конкретизировать
        this.getController('tpm.promo.Promo').detailButton = null;
        promoeditorcustom.isFromSchedule = schedulerData;

        //Если одно ограничение - сразу выбираем клиента
        var userRoleConstrains = App.UserInfo.getConstrains();
        me.defaultClientTree = null;
        if (Object.keys(userRoleConstrains).length == 1) {
            $.ajax({
                dataType: 'json',
                type: 'POST',
                url: '/odata/ClientTrees/GetClientTreeByObjectId?objectId=' + Object.keys(userRoleConstrains)[0],
                async: false,
                success: function (data) {
                    var result = Ext.JSON.decode(data.value);
                    if (result.data.IsBaseClient) {
                        me.defaultClientTree = result.data;
                    }
                },
                error: function () {
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }
            });
        }

        // установка флага Apollo Export
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var apolloExportCheckbox = promoeditorcustom.down('[name=ApolloExportCheckbox]');
        apolloExportCheckbox.setValue(true);

        if (apolloExportCheckbox.crudAccess.indexOf(currentRole) === -1) {
            apolloExportCheckbox.setReadOnly(true);
        }

        // для блокировки/разблокировки грида/календаря
        // Если кнопка не null, то из грида, иначе из календаря
        var parentWidget = button ? button.up('promo') : Ext.ComponentQuery.query('schedulecontainer')[0];
        parentWidget.setLoading(true);

        if (schedulerData && schedulerData.isCopy) {
            this.bindAllLoadEvents(promoeditorcustom, schedulerData, true);
            me.fillPromoForm(promoeditorcustom, schedulerData, false, true);
        } else {
            this.getScrollPosition(button);

            this.bindAllLoadEvents(promoeditorcustom, null, false);
            $.ajax({
                dataType: 'json',
                url: '/odata/PromoStatuss',
                success: function (promoStatusData) {
                    if (!promoeditorcustom.isDestroyed) {
                        var client = promoeditorcustom.down('container[name=promo_step1]');
                        var product = promoeditorcustom.down('container[name=promo_step2]');
                        var mechanic = promoeditorcustom.down('container[name=promo_step3]');
                        var period = promoeditorcustom.down('container[name=promo_step4]');
                        var budgetYear = promoeditorcustom.down('container[name=promo_step5]');
                        var event = promoeditorcustom.down('container[name=promo_step6]');
                        var settings = promoeditorcustom.down('container[name=promo_step7]');
                        var adjustment = promoeditorcustom.down('container[name=promo_step8]');

                        //disable On/Off invoice group
                        client.down('[id=OffInvoice]').setDisabled(true);
                        client.down('[id=OnInvoice]').setDisabled(true);

                        // Кнопки для изменения состояний промо
                        var promoActions = Ext.ComponentQuery.query('button[isPromoAction=true]');


                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');

                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');

                        // mechanic
                        var promoController = App.app.getController('tpm.promo.Promo');
                        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
                        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
                        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
                        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
                        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');
                        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');
                        var promoComment = mechanic.down('textarea[name=PromoComment]');
                        var isEdit = false;
                        var isReadOnly = false;

                        promoController.mechanicTypeChange(
                            marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
                            promoController.getMechanicListForUnlockDiscountField(),
                            isReadOnly, isEdit
                        );

                        promoController.mechanicTypeChange(
                            instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
                            promoController.getMechanicListForUnlockDiscountField(),
                            isReadOnly, isEdit
                        );

                        // event
                        me.refreshPromoEvent(promoeditorcustom, false);

                        // settings
                        settings.down('sliderfield[name=priority]').setValue(3);
                        var promoEventButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step7]')[0];
                        promoEventButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep7') + '</b><br><p> Calendar priority: ' + 3 + '</p>');
                        promoEventButton.removeCls('notcompleted');
                        promoEventButton.setGlyph(0xf133);

                        // Установки Adjustment   
                        adjustment.down('sliderfield[name=DeviationCoefficient]').setDisabled(true);
                        adjustment.down('numberfield[name=Adjustment]').setReadOnly(true);
                        adjustment.down('numberfield[name=Adjustment]').addCls('readOnlyField');

                        adjustment.down('sliderfield[name=DeviationCoefficient]').setValue(0);
                        adjustment.down('numberfield[name=Adjustment]').setValue(0);
                        var promoAdjustmentButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step8]')[0];
                        promoAdjustmentButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep8') + '</b><br><p>' + l10n.ns('tpm', 'Promo').value('Adjustment') + ': ' + 0 + '%' + '</p>');
                        promoAdjustmentButton.removeCls('notcompleted');
                        promoAdjustmentButton.setGlyph(0xf133);

                        // если создание из календаря
                        if (schedulerData) {
                            var durationDateStart = period.down('datefield[name=DurationStartDate]');
                            var durationDateEnd = period.down('datefield[name=DurationEndDate]');
                            var startDate = schedulerData.schedulerContext.start;
                            var endDate = schedulerData.schedulerContext.end;

                            durationDateStart.setValue(startDate);
                            durationDateEnd.setValue(endDate);
                        }

                        var clientRecord;
                        if (schedulerData && !schedulerData.isCopy) {
                            clientRecord = schedulerData.schedulerContext.resourceRecord.raw;
                        }
                        else if (me.defaultClientTree != null) {
                            clientRecord = me.defaultClientTree;
                        }

                        if (clientRecord) {
                            var promoClientForm = promoeditorcustom.down('container[name=promo_step1]');
                            promoClientForm.fillForm(clientRecord, false);
                            me.checkParametersAfterChangeClient(clientRecord, promoeditorcustom);
                            if (schedulerData) {
                                me.afterInitClient(clientRecord, schedulerData.schedulerContext.resourceRecord, promoeditorcustom, schedulerData.isCopy);
                            }

                            var promoActivityMechanic = promoeditorcustom.down('container[name=promoActivity_step1]');
                            var actualMechanicId = promoActivityMechanic.down('[name=ActualInstoreMechanicId]');
                            marsMechanicId.setDisabled(false);
                            instoreMechanicId.setDisabled(false);
                            actualMechanicId.setDisabled(false);
                        }

                        for (var i = 0; i < promoStatusData.value.length; i++) {
                            if (promoStatusData.value[i].SystemName == 'Draft') {
                                promoeditorcustom.statusId = promoStatusData.value[i].Id;
                                promoeditorcustom.promoStatusName = promoStatusData.value[i].Name;
                                promoeditorcustom.promoStatusSystemName = promoStatusData.value[i].SystemName;
                                promoeditorcustom.promoName = 'Unpublish Promo';

                                me.setPromoTitle(promoeditorcustom, promoeditorcustom.promoName, promoeditorcustom.promoStatusName);
                                me.defineAllowedActions(promoeditorcustom, promoActions, promoeditorcustom.promoStatusName);

                                var undoBtn = promoeditorcustom.down('button[itemId=btn_undoPublish]');
                                undoBtn.statusId = promoStatusData.value[i].Id;
                                undoBtn.statusName = promoStatusData.value[i].Name;
                                undoBtn.statusSystemName = promoStatusData.value[i].SystemName;
                                
                            }

                            if (promoStatusData.value[i].SystemName == 'DraftPublished') {
                                var btn = promoeditorcustom.down('button[itemId=btn_publish]');
                                btn.statusId = promoStatusData.value[i].Id;
                                btn.statusName = promoStatusData.value[i].Name;
                                btn.statusSystemName = promoStatusData.value[i].SystemName;
                                var btn_backToDraftPublished = promoeditorcustom.down('button[itemId=btn_backToDraftPublished]');
                                btn_backToDraftPublished.statusId = promoStatusData.value[i].Id;
                                btn_backToDraftPublished.statusName = promoStatusData.value[i].Name;
                                btn_backToDraftPublished.statusSystemName = promoStatusData.value[i].SystemName;
                            }

                            if (promoStatusData.value[i].SystemName == 'OnApproval') {
                                var btn = promoeditorcustom.down('button[itemId=btn_sendForApproval]');
                                btn.statusId = promoStatusData.value[i].Id;
                                btn.statusName = promoStatusData.value[i].Name;
                                btn.statusSystemName = promoStatusData.value[i].SystemName;
                            }

                            if (promoStatusData.value[i].SystemName == 'Approved') {
                                var btn_approve = promoeditorcustom.down('button[itemId=btn_approve]');
                                btn_approve.statusId = promoStatusData.value[i].Id;
                                btn_approve.statusName = promoStatusData.value[i].Name;
                                btn_approve.statusSystemName = promoStatusData.value[i].SystemName;
                            }

                            if (promoStatusData.value[i].SystemName == 'Cancelled') {
                                var btn_cancel = promoeditorcustom.down('button[itemId=btn_cancel]');
                                btn_cancel.statusId = promoStatusData.value[i].Id;
                                btn_cancel.statusName = promoStatusData.value[i].Name;
                                btn_cancel.statusSystemName = promoStatusData.value[i].SystemName;
                            }

                            if (promoStatusData.value[i].SystemName == 'Planned') {
                                var btn_plan = promoeditorcustom.down('button[itemId=btn_plan]');
                                btn_plan.statusId = promoStatusData.value[i].Id;
                                btn_plan.statusName = promoStatusData.value[i].Name;
                                btn_plan.statusSystemName = promoStatusData.value[i].SystemName;
                            }

                            if (promoStatusData.value[i].SystemName == 'Closed') {
                                var btn_close = promoeditorcustom.down('button[itemId=btn_close]');
                                btn_close.statusId = promoStatusData.value[i].Id;
                                btn_close.statusName = promoStatusData.value[i].Name;
                                btn_close.statusSystemName = promoStatusData.value[i].SystemName;
                            }

                            if (promoStatusData.value[i].SystemName == 'Finished') {
                                var btn_backToFinished = promoeditorcustom.down('button[itemId=btn_backToFinished]');
                                btn_backToFinished.statusId = promoStatusData.value[i].Id;
                                btn_backToFinished.statusName = promoStatusData.value[i].Name;
                                btn_backToFinished.statusSystemName = promoStatusData.value[i].SystemName;
                            }
                        }

                        promoeditorcustom.show();
                        parentWidget.setLoading(false);
                        me.checkLoadingComponents();
                    } else {
                        parentWidget.setLoading(false);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    parentWidget.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                }
            });
        }
    },

    onCreateInOutButtonClick: function (button, e, schedulerData, promotype) {

        var me = this;
        e.isInOutPromo = true;
        me.onCreateButtonClick(button, e, schedulerData, true, promotype);
    },
    onCreateRegularButtonClick: function (button, e, schedulerData, promotype) {
        var me = this;
        me.onCreateButtonClick(button, e, schedulerData, false, promotype);
    },
    onCreateLoyaltyButtonClick: function (button, e, schedulerData, promotype) {
        var me = this;
        me.onCreateButtonClick(button, e, schedulerData, false, promotype);
    },
    onCreateDynamicButtonClick: function (button, e, schedulerData, promotype) {
        var me = this;
        me.onCreateButtonClick(button, e, schedulerData, false, promotype);
    },
    onSelectionButtonClick: function (button) {
        var window = button.up('window');
        var fieldsetWithButtons = window.down('fieldset');

        fieldsetWithButtons.items.items.forEach(function (item) {
            item.down('button').up('container').removeCls('promo-type-select-list-container-button-clicked');
            item.down('button').addCls('promo-type-select-list-container-button-shplack');
        });

        button.up('container').addCls('promo-type-select-list-container-button-clicked');
        button.removeCls('promo-type-select-list-container-button-shplack');
        window.selectedButton = button;
    },
    onPromoTypeOkButtonClick: function (button, e) {
        var me = this;
        var window = button.up('window');

        var promoButton = Ext.ComponentQuery.query('promo')[0].down('#createbutton');
        if (window.selectedButton != null) {

            var selectedButtonText = window.selectedButton.budgetRecord;

            var method = "onCreate" + selectedButtonText.SystemName + "ButtonClick";
            if (me[method] != undefined) {
                me[method](promoButton, e, null, selectedButtonText);
                window.close();
            } else {
                App.Notify.pushError('Не найдено типа промо ' + selectedButtonText.Name);
            }
        } else {

            App.Notify.pushError(l10n.ns('tpm', 'PromoTypes').value('NoPromoType'));
        }

    },
    getPromoType: function () {
        var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        if (promoeditorcustom && promoeditorcustom.promotype) {
            return promoeditorcustom.promotype;
        } else {
            return false;
        }
    },
    setPromoType: function (promotype, promoeditorcustom) {
        promoeditorcustom.promotype = promotype;
    },
    onPromoTypeAfterRender: function (window) {
        var closeButton = window.down('#close');
        var okButton = window.down('#ok');

        //remove type

        closeButton.setText(l10n.ns('tpm', 'PromoType').value('ModalWindowCloseButton'));
        okButton.setText(l10n.ns('tpm', 'PromoType').value('ModalWindowOkButton'));
        window.selectedButton = null;
    },

    onAllCreateButtonClick: function (button) {

        var supportType = Ext.widget('promotypewindow');
        var mask = new Ext.LoadMask(supportType, { msg: "Please wait..." });


        supportType.show();
        mask.show();

        supportType.createPromoSupportButton = button;
        //remove type
        var query = breeze.EntityQuery
            .from('PromoTypes')
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                if (data.httpResponse.data.results.length > 0) {
                    supportType.down('#promotypewindowInnerContainer').show();
                    data.httpResponse.data.results.forEach(function (item) {
                        // Контейнер с кнопкой (обводится бордером при клике)
                        var promoTypeItem = Ext.widget({
                            extend: 'Ext.container.Container',
                            width: 'auto',
                            xtype: 'container',
                            layout: {
                                type: 'hbox',

                                align: 'stretch'
                            },
                            items: [{
                                xtype: 'button',
                                enableToggle: true,
                                cls: 'promo-type-select-list-button',
                            }]
                        });

                        promoTypeItem.addCls('promo-type-select-list-container-button');
                        //promoTypeItem.down('button').style = {borderLeft: '6px solid ' + 'rgb(179, 193, 210)'};  
                        promoTypeItem.down('button').addCls('promo-type-select-list-container-button-shplack');
                        promoTypeItem.down('button').setText(item.Name);
                        promoTypeItem.down('button').renderData.glyphCls = 'promo-type-select-list-button';
                        promoTypeItem.down('button').setGlyph(parseInt('0x' + item.Glyph, 16));
                        promoTypeItem.down('button').budgetRecord = item;
                        supportType.down('fieldset').add(promoTypeItem);

                    });
                } else {
                    Ext.ComponentQuery.query('promotypewindow')[0].close();
                    App.Notify.pushError('Не найдено записей   типа промо ');
                }

                mask.hide();
            })
            .fail(function () {
                App.Notify.pushError('Ошибка при выполнении операции');
                mask.hide();
            })


    },
    onUpdateButtonClick: function (button) {
        var me = this;
        var grid = this.getGridByButton(button);
        grid.up('promo').setLoading(true);
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        me.detailButton = null;
        var promoStatusName = null;
        var record = me.getRecord(promoeditorcustom);

        // Если запись обозначена
        if (button.assignedRecord) {
            promoeditorcustom.isCreating = false;
            promoeditorcustom.assignedRecord = button.assignedRecord;

            this.bindAllLoadEvents(promoeditorcustom, button.assignedRecord, false);
            this.fillPromoForm(promoeditorcustom, button.assignedRecord, false, false, grid);

            promoStatusName = button.assignedRecord.get('PromoStatusName');
            // Редактирование выбранной записи
        } else {
            me.getScrollPosition(button);

            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var record = selectionModel.getSelection()[0];
                grid.promoStore.load({
                    id: record.getId(),
                    scope: me,
                    callback: function (records, operation, success) {
                        if (success) {
                            record = records[0];
                            promoeditorcustom.isCreating = false;
                            promoeditorcustom.model = record;
                            me.bindAllLoadEvents(promoeditorcustom, record, false);
                            me.fillPromoForm(promoeditorcustom, record, false, false, grid);
                            promoStatusName = record.get('PromoStatusName');
                        }
                    }
                });
            } else {
                grid.up('promo').setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('notSelected'));
            }
        }

        //Для блокирования кнопки продуктов
        promoeditorcustom.productsSetted = true;


        //Установка readOnly полям, для которых текущая роль не входит в crudAccess
        me.setFieldsReadOnlyForSomeRole(promoeditorcustom);

        //var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
        //var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

        //if (record.data.InOut) {
        //	needRecountUplift.setDisabled(true);
        //} else {
        //	needRecountUplift.setDisabled(false);
        //}

        //if (needRecountUplift.value === true) {
        //          planUplift.changeEditable(true);
        //          planUplift.up('container').setReadable(true);
        //          planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33e);
        //      } else {
        //          planUplift.changeEditable(false);
        //          planUplift.up('container').setReadable(false);
        //          planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33f);
        //}

        //Начавшиеся promo не редактируются период (кроме роли Support Administrator)
        var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoStatusName) >= 0);
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (isPromoWasStarted && currentRole !== 'SupportAdministrator') {
            me.blockStartedPromoDateChange(promoeditorcustom, me);
        }

        //Начавшиеся promo не редактируются uplift (кроме роли Support Administrator)
        //      var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoStatusName) >= 0);
        //      if (isPromoWasStarted && currentRole !== 'SupportAdministrator') {
        //	me.blockStartedPromoUplift();
        //}

        me.setFieldsReadOnlyForSomeRole(promoeditorcustom);
    },

    onDetailButtonClick: function (button) {
        var me = this;
        var promoeditorcustom = Ext.widget('promoeditorcustom');
        me.getController('tpm.promo.Promo').detailButton = button;

        // Если запись обозначена
        if (button.assignedRecord) {
            promoeditorcustom.isCreating = false;
            promoeditorcustom.assignedRecord = button.assignedRecord;

            me.bindAllLoadEvents(promoeditorcustom, button.assignedRecord, false);
            me.fillPromoForm(promoeditorcustom, button.assignedRecord, true);
            // Просмотр выбранной записи
        } else {
            this.getScrollPosition(button);
            var grid = me.getGridByButton(button);
            grid.up('promo').setLoading(true);

            var selectionModel = grid.getSelectionModel();
            if (selectionModel.hasSelection()) {
                var record = selectionModel.getSelection()[0];
                grid.promoStore.load({
                    id: record.getId(),
                    scope: me,
                    callback: function (records, operation, success) {
                        if (success) {
                            record = records[0];
                            promoeditorcustom.isCreating = false;
                            promoeditorcustom.model = record;
                            me.bindAllLoadEvents(promoeditorcustom, record, false);
                            me.fillPromoForm(promoeditorcustom, record, true, false, grid);
                        }
                    }
                });
            } else {
                App.Notify.pushError(l10n.ns('tpm', 'text').value('notSelected'));
                grid.up('promo').setLoading(false);
            }
            var cancelButton = promoeditorcustom.down('#cancelPromo');
            cancelButton.promoGridDetailMode = grid;
        }
    },

    onSavePromoButtonClick: function (button) {
        this.savePromo(button, false, true);
    },

    onSplitAndPublishButtonClick: function (button) {
        this.splitAndPublishPromo(button, this);
    },

    onSaveAndClosePromoButtonClick: function (button) {
        this.savePromo(button, true);
    },

    // Кнопка редактирования промо (Edit button)
    onChangePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        window.setLoading(true);
        // отложенный вызов функции изменения состояния формы для того чтобы сначала отрисовался лоадер
        Ext.Function.defer(this.setEditingAfterSetLoading, 1, this, [button]);
    },

    // Перевод формы промо в режим редактирования
    setEditingAfterSetLoading: function (button) {
        Ext.suspendLayouts();
        var me = this,
            promoeditorcustom = button.up('window');
        promoeditorcustom.readOnly = false;
        var record = me.getRecord(promoeditorcustom);

        var promoActivity = promoeditorcustom.down('promoactivity');

        var promoClientForm = promoeditorcustom.down('container[name=promo_step1]');
        var promoProductForm = promoeditorcustom.down('container[name=promo_step2]');
        var mechanic = promoeditorcustom.down('container[name=promo_step3]');
        var promoActivityStep1 = promoActivity.down('container[name=promoActivity_step1]');
        var promoBudgets = button.up('window').down('promobudgets');

        me.setReadOnlyForChildrens(promoActivity, promoeditorcustom.promoStatusSystemName, true, record.data.InOut);
        me.setReadOnlyForChildrens(promoBudgets, promoeditorcustom.promoStatusSystemName, true, record.data.InOut);

        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];

        // --------------- basic promo ---------------

        // Promo Client
        var clientCrudAccess = ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        if (clientCrudAccess.indexOf(currentRole) > -1) {

            promoClientForm.down('#choosePromoClientBtn').setDisabled(false);
            promoClientForm.down('[id=OffInvoice]').setReadOnly(false);
            promoClientForm.down('[id=OnInvoice]').setReadOnly(false);
        }

        // Product tree
        var productCrudAccess = ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        var window = button.up('promoeditorcustom');
        if (productCrudAccess.indexOf(currentRole) > -1) {
            window.productsSetted = true;
            promoProductForm.setDisabledBtns(false);
        }

        // --------------- buttons ---------------

        //uplift
        promoeditorcustom.down('button[itemId=savePromo]').show();
        promoeditorcustom.down('button[itemId=saveAndClosePromo]').show();
        promoeditorcustom.down('button[itemId=cancelPromo]').show();
        promoeditorcustom.down('button[itemId=closePromo]').hide();
        promoeditorcustom.down('button[itemId=changePromo]').hide();

        //Установка readOnly полям, для которых текущая роль не входит в crudAccess
        me.setFieldsReadOnlyForSomeRole(promoeditorcustom);

        //var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
        //var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

        //if (record.data.InOut) {
        //	needRecountUplift.setDisabled(true);
        //} else {
        //	needRecountUplift.setDisabled(false);
        //}

        //      if (needRecountUplift.value === true) {
        //          planUplift.changeEditable(true);
        //          planUplift.up('container').setReadable(true);
        //          planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33e);
        //      } else {
        //          planUplift.changeEditable(false);
        //          planUplift.up('container').setReadable(false);
        //          planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33f);
        //}

        me.validatePromoModel(promoeditorcustom);

        // Разблокировка кнопок Add Promo Support
        //var addSubItemButtons = promoBudgets.query('#addSubItem');

        //addSubItemButtons.forEach(function (button) {
        //    button.setDisabled(false);
        //});

        // Сброc полей Instore Mechanic
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
            mechanicFields = promoController.getMechanicFields(promoMechanic);

        promoController.disableFields([
            mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
            mechanicFields.instoreMechanicFields.instoreMechanicDiscount
        ]);

        mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
            mechanicFields.instoreMechanicFields.instoreMechanicDiscount

        //Установка полей механик
        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');
        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');
        var isEdit = true;
        var isReadOnly = false;

        promoController.mechanicTypeChange(
            marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            isReadOnly, isEdit
        );

        promoController.mechanicTypeChange(
            instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            isReadOnly, isEdit
        );

        //Actual механники
        var actualInstoreMechanicId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actualInstoreMechanicTypeId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicTypeId]');
        var actualInStoreDiscount = promoActivityStep1.down('numberfield[name=ActualInStoreDiscount]');

        promoController.mechanicTypeChange(
            actualInstoreMechanicId, actualInstoreMechanicTypeId, actualInStoreDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            isReadOnly, isEdit
        );

        //Начавшиеся promo не редактируются период
        var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoeditorcustom.promoStatusName) >= 0);
        if (isPromoWasStarted && currentRole !== 'SupportAdministrator') {
            me.blockStartedPromoDateChange(promoeditorcustom, me);
        }

        ////Начавшиеся promo не редактируются uplift
        //var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoeditorcustom.promoStatusName) >= 0);
        //if (isPromoWasStarted) {
        //	me.blockStartedPromoUplift();
        //}

        if (promoeditorcustom.promoStatusName === 'Closed') {
            promoeditorcustom.down('promobudgetyear').down('combobox').setReadOnly(true);
        }
        // Если Промо в статусе от Started, то заблокировать редактирование PromoBasic
        if (isPromoWasStarted && currentRole !== 'SupportAdministrator') {
            var promoEvent = promoeditorcustom.down('container[name=promo_step6]').down('chooseEventButton');
            var priority = promoeditorcustom.down('container[name=promo_step7]').down('sliderfield[name=priority]');


            // --------------- basic promo ---------------
            // client
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);
            promoClientForm.down('[id=OnInvoice]').setReadOnly(true);
            promoClientForm.down('[id=OffInvoice]').setReadOnly(true);

            // product
            promoProductForm.setDisabledBtns(true);

            // event
            promoEvent.setDisabled(true); // button

            // settings
            priority.setReadOnly(true);

            var elementsToReadOnly = promoeditorcustom.down('container[itemId=basicPromoPanel]').query('[needReadOnly=true]');
            me.setReadOnlyProperty(true, elementsToReadOnly);

            promoController.mechanicTypeChange(
                marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                true
            );

            promoController.mechanicTypeChange(
                instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                true
            );
        }
        // Заблокировать IsApolloExport для редактирования в указанных ниже статусах
        var isPromoEnd = (['Finished', 'Closed', 'Cancelled', 'Deleted'].indexOf(record.data.PromoStatusSystemName) >= 0);
        if (isPromoEnd) {
            promoeditorcustom.down('[name=ApolloExportCheckbox]').setReadOnly(true);
        }
        if (record.data.PromoStatusSystemName === 'Started'
            && promoeditorcustom.down('[name=ApolloExportCheckbox]').crudAccess.indexOf(App.UserInfo.getCurrentRole()['SystemName']) >= 0) {
            promoeditorcustom.down('[name=ApolloExportCheckbox]').setDisabled(false);
            promoeditorcustom.down('[name=ApolloExportCheckbox]').setReadOnly(false);
        }

        // Заблокировать Adjusment для редактирования в указанных ниже статусах
        var isPromoEnd = (['Finished', 'Closed', 'Cancelled', 'Deleted'].indexOf(record.data.PromoStatusSystemName) >= 0);
        if (isPromoEnd
            || promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').crudAccess.indexOf(App.UserInfo.getCurrentRole()['SystemName']) === -1) {
            promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').setDisabled(true);
            promoeditorcustom.down('numberfield[name=Adjustment]').setReadOnly(true);
            promoeditorcustom.down('numberfield[name=Adjustment]').addCls('readOnlyField');
        }
        if ((record.data.PromoStatusSystemName === 'Started'
            && promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').crudAccess.indexOf(App.UserInfo.getCurrentRole()['SystemName']) >= 0)
            || ((['Finished', 'Closed'].indexOf(record.data.PromoStatusSystemName) >= 0)
                && App.UserInfo.getCurrentRole()['SystemName'] === 'SupportAdministrator')) {
            promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').setDisabled(false);
            promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').setReadOnly(false);
            promoeditorcustom.down('numberfield[name=Adjustment]').setDisabled(false);
            promoeditorcustom.down('numberfield[name=Adjustment]').setReadOnly(false);
            promoeditorcustom.down('numberfield[name=Adjustment]').removeCls('readOnlyField');
        }
        // Для Growth Acceleration Promo
        var growthAccelerationCheckbox = promoeditorcustom.down('[name=GrowthAccelerationCheckbox]');
        var gaReadOnlyStatuses = ['Approved', 'Planned', 'Started', 'Finished'];
        if (gaReadOnlyStatuses.indexOf(promoeditorcustom.promoStatusSystemName) != -1 && currentRole !== 'SupportAdministrator') {
            growthAccelerationCheckbox.setReadOnly(true);
        }

        var promoComment = mechanic.down('textarea[name=PromoComment]');
        var pcReadOnlyStatuses = ['Closed', 'Deleted', 'Cancelled'];
        if ((pcReadOnlyStatuses.indexOf(promoeditorcustom.promoStatusSystemName) == -1 && promoComment.crudAccess.indexOf(currentRole) != -1)
            || currentRole == 'SupportAdministrator') {
            promoComment.setReadOnly(false);
        }

        // редактирование Add TI Approved для не approved
        //var isApproved = ['Approved', 'Planned', 'Started', 'Finished'].includes(promoeditorcustom.promoStatusSystemName);
        var isApproved = record.data.LastApprovedDate != null;
        if (!isApproved) {
            promoeditorcustom.down('panel[name=promoBudgets_step4]').down('numberfield[name=PlanAddTIMarketingApproved]').setReadOnly(false);
        }

        me.checkLoadingComponents();
        promoeditorcustom.setLoading(false);
        Ext.resumeLayouts(true);
    },

    blockStartedPromoDateChange: function (promoeditorcustom, me) {
        var period = promoeditorcustom.down('container[name=promo_step4]');
        var durationStartDate = period.down('datefield[name=DurationStartDate]');
        var durationEndDate = period.down('datefield[name=DurationEndDate]');
        var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
        var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');
        var dateFields = [durationStartDate, durationEndDate, dispatchStartDate, dispatchEndDate];
        me.setReadOnlyProperty(true, dateFields);
        for (var i = 0; i < dateFields.length; i++) {
            dateFields[i].setReadOnly(true);
            dateFields[i].addCls('readOnlyField');
        }
    },

    //blockStartedPromoUplift: function () {
    //	var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
    //	var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];
    //	needRecountUplift.setDisabled(true);
    //	needRecountUplift.setReadOnly(true);
    //       needRecountUplift.addCls('readOnlyField');
    //       planUplift.changeEditable(false);
    //       planUplift.up('container').setReadable(true);
    //       planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33f);
    //},

    onCancelPromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');
        var promoeditorcustom = button.up('window');
        promoeditorcustom.setLoading(true, undefined, true);
        var me = this;
        setTimeout(function () {

            // Если есть открытый календарь - обновить его
            var calendarGrid = Ext.ComponentQuery.query('scheduler');
            if (calendarGrid.length > 0) {
                calendarGrid[0].resourceStore.load();
            }

            if (window) {
                promoeditorcustom.tempEditUpliftId = null;
                promoeditorcustom.productUpliftChanged = false;
                var grid = button.promoGridDetailMode;
                if (grid) {
                    //var selectionModel = grid.getSelectionModel();
                    //if (selectionModel.hasSelection()) {
                    //    var record = selectionModel.getSelection()[0];
                    //    window.readOnly = true;
                    //    me.reFillPromoForm(window, record, grid);
                    //}
                    window.readOnly = true;
                    me.reFillPromoForm(window, window.model, grid);
                } else {
                    window.close();
                }
            }
        }, 5);
        promoeditorcustom.setLoading(false, undefined, false);
    },

    onClosePromoButtonClick: function (button) {
        var window = button.up('promoeditorcustom');

        if (window) {         
            window.close();
        }
    },

    onPromoHistoryButtonClick: function (button) {
        var choosepromowindow = Ext.create('App.view.core.base.BaseReviewWindow', {
            title: l10n.ns('tpm', 'customtoptoolbar').value('customHistory'),
            width: "95%",
            height: "95%",
            minWidth: 800,
            minHeight: 600,
            layout: 'fit',
            id: 'promoHistoryChanges',
            items: [{
                xtype: 'customhistoricalpromo'
            }]
        });

        var window = button.up('window');
        var grid = choosepromowindow.down('grid');
        var store = grid.getStore(),
            proxy = store.getProxy();
        proxy.extraParams.promoIdHistory = window.promoId;

        store.on({
            load: function (records, operation, success) {
                var selModel = grid.getSelectionModel();

                if (!selModel.hasSelection() && records.data.length > 0) {
                    selModel.select(0);
                    grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                } else if (selModel.hasSelection() && records.data.length > 0) {
                    var selected = selModel.getSelection()[0];
                    if (store.indexOfId(selected.getId()) === -1) {
                        selModel.select(0);
                        grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                    }
                } else if (records.data.length === 0) {
                    selModel.deselectAll();
                }

                grid.setLoading(false);
            }
        });

        choosepromowindow.show(false, function () {
            grid.setLoading(true);
        });

        store.load();

        //Корректировка области прокрутки
        var h = choosepromowindow.down('[itemId=datatable]').getHeight();
        choosepromowindow.down('custompromopanel').setHeight(h - 34);
    },

    updateStatusHistoryState: function () {
        // Обновляем дерево статусов
        var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var approvalhistory = promoeditorcustom.down('container[name=changes]');
        var panel = approvalhistory.down('panel');

        var parameters = {
            promoKey: promoeditorcustom.promoId,
        }
        App.Util.makeRequestWithCallback('PromoStatusChanges', 'PromoStatusChangesByPromo', parameters, function (data) {
            var result = Ext.JSON.decode(data.httpResponse.data.value);
            if (result.success) {
                if (!promoeditorcustom.isDestroyed) {
                    promoeditorcustom.fireEvent('fillWFPanel', panel, result, approvalhistory, promoeditorcustom);
                }
            } else {
                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
            }

        });
    },

    // =============== Other ===============

    //Общая валидация всех полей промо
    validatePromoModel: function (window) {
        var errorMessage = '';
        var errorSecondLayer = '';
        var errorTrirdLayer = '';

        var isStep1Complete = !window.down('#btn_promo_step1').hasCls('notcompleted');
        var isStep2Complete = !window.down('#btn_promo_step2').hasCls('notcompleted');

        var promomechanic = window.down('promomechanic');
        var promoperiod = window.down('promoperiod');
        var promosettings = window.down('promosettings');
        var promoactivity = window.down('promoactivity');
        var currentStatusName = window.promoStatusName;

        //Basic
        if (['Started', 'Finished', 'Closed'].indexOf(currentStatusName) < 0) {
            //Step 1
            if (!isStep1Complete)
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep1Validate') + "; ";
            //Step 2
            if (!isStep2Complete)
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep2Validate') + "; ";
            //Step 3
            if (!promomechanic.down('searchcombobox[name=MarsMechanicId]').validate()
                || !promomechanic.down('searchcombobox[name=MarsMechanicTypeId]').validate()
                || !promomechanic.down('numberfield[name=MarsMechanicDiscount]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('MarsMechanicValidate') + ", ";

            if (!promomechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]').validate()
                || !promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('PlanInstoreMechanicValidate') + ", ";

            if (!promomechanic.down('textarea[name=PromoComment]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('PromoCommentValidate') + ", ";

            if (errorTrirdLayer != '' || window.down('#btn_promo_step3').hasCls('notcompleted')) {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep3Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }

            //Step 4
            if (!promoperiod.down('datefield[name=DurationStartDate]').validate()
                || !promoperiod.down('datefield[name=DurationEndDate]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('DurationDateValidate') + ", ";

            if (!promoperiod.down('datefield[name=DispatchStartDate]').validate()
                || !promoperiod.down('datefield[name=DispatchEndDate]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('DispatchValidate') + ", ";

            if (errorTrirdLayer != '' || window.down('#btn_promo_step4').hasCls('notcompleted')) {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep4Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }

            //Step 5
            if (window.down('#btn_promo_step5').hasCls('notcompleted')) {
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep5Validate') + "; ";
            }

            //Step 6
            if (window.down('#btn_promo_step6').hasCls('notcompleted')) {
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep6Validate') + "; ";
            }

            //Step 7
            if (window.down('#btn_promo_step7').hasCls('notcompleted')) {
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep7Validate') + "; ";
            }

            //Step 8
            if (window.down('#btn_promo_step8').hasCls('notcompleted')) {
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep8Validate') + "; ";
            }
        }


        //End basic validation
        if (errorSecondLayer != '') {
            if (errorSecondLayer.endsWith('; ')) errorSecondLayer = errorSecondLayer.replace(/..$/, '. ');
            errorMessage += l10n.ns('tpm', 'text').value('completeBasicValidate') + ': ' + errorSecondLayer;
            errorSecondLayer = ''
        }

        if (!window.down('numberfield[name=PlanAddTIMarketingApproved]').validate()) {
            errorMessage += l10n.ns('tpm', 'text').value('completeBudgetsStep4Validate');
        }

        //В статусе Draft Activity заблокировано, нет смысла валидировать
        if (['Draft'].indexOf(currentStatusName) < 0) {
            //Activity - step 1
            if (!promoactivity.down('searchcombobox[name=ActualInstoreMechanicId]').validate()
                || !promoactivity.down('searchcombobox[name=ActualInstoreMechanicTypeId]').validate()
                || !promoactivity.down('numberfield[name=ActualInStoreDiscount]').validate()
                || !promoactivity.down('numberfield[name=ActualInStoreShelfPrice]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('ActualInstoreAssumptionValidate') + ", ";

            if (!promoactivity.down('numberfield[name=PlanInStoreShelfPrice]').validate())
                errorTrirdLayer += l10n.ns('tpm', 'text').value('PlanInstoreAssumptionValidate') + ", ";

            if (errorTrirdLayer != '') {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep1Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }

            //Activity - step 2
            if (!promoactivity.down('triggerfielddetails[name=PlanPromoUpliftPercent]').validate()) {
                errorTrirdLayer += l10n.ns('tpm', 'text').value('PlanPromoUpliftPercentError') + ", ";
            }

            if (!promoactivity.down('triggerfielddetails[name=SumInvoice]').validate()) {
                errorTrirdLayer += l10n.ns('tpm', 'text').value('SumInvoiceValidate') + ", ";
            }

            if (!promoactivity.down('textfield[name=InvoiceNumber]').validate()) {
                errorTrirdLayer += l10n.ns('tpm', 'text').value('InvoiceNumberValidate') + ", ";
            }
            if (!promoactivity.down('textfield[name=DocumentNumber]').validate()) {
                errorTrirdLayer += l10n.ns('tpm', 'text').value('DocumentNumberValidate') + ", ";
            }
            if (errorTrirdLayer != '') {
                if (errorTrirdLayer.endsWith(', ')) errorTrirdLayer = errorTrirdLayer.replace(/..$/, '; ');
                errorSecondLayer += l10n.ns('tpm', 'text').value('completeStep2Validate') + ' - ' + errorTrirdLayer;
                errorTrirdLayer = '';
            }

            //End activity validation
            if (errorSecondLayer != '') {
                if (errorSecondLayer.endsWith('; ')) errorSecondLayer = errorSecondLayer.replace(/..$/, '. ');
                errorMessage += l10n.ns('tpm', 'text').value('completeActivityValidate') + ': ' + errorSecondLayer;
                errorSecondLayer = ''
            }
        }

        //End validation
        if (errorMessage != '')
            return l10n.ns('tpm', 'text').value('completeStepsValidate') + errorMessage;

        else return errorMessage;
    },

    buildPromoModel: function (window, record) {
        // basic promo
        var promomechanic = window.down('promomechanic');
        var promoperiod = window.down('promoperiod');
        var promobudgetyear = window.down('promobudgetyear');
        var promoevent = window.down('promoevent');
        var promosettings = window.down('promosettings');
        var promoadjustment = window.down('promoadjustment');

        // promo budgets
        var promoBudgets = window.down('promobudgets');

        // calculation
        //var promocalculation = window.down('promocalculation');

        // promo activity
        var promoActivity = window.down('promoactivity');

        var promoClientForm = window.down('promoclient');
        // --------------- basic promo ---------------
        record.data.PromoStatusId = window.statusId;
        record.data.ClientId = null;

        // promo client
        record.data.ClientTreeId = window.clientTreeId;
        record.data.ClientHierarchy = window.clientHierarchy;
        record.data.ClientTreeKeyId = window.clientTreeKeyId;
        record.data.IsOnInvoice = promoClientForm.getInvoiceType();
        //if (promoClientForm.clientTreeRecord.InOut !== null && promoClientForm.clientTreeRecord.InOut !== undefined && (window.clientTreeKeyId - 10002 > 0)) {
        //	record.data.ClientTreeKeyId = promoClientForm.clientTreeRecord.InOut ? window.clientTreeKeyId - 10002 : window.clientTreeKeyId - 10001;
        //} else {
        //	record.data.ClientTreeKeyId = window.clientTreeKeyId;
        //}

        // producttree
        record.data.BrandId = window.brandId;
        record.data.TechnologyId = window.technologyId;
        record.data.ProductHierarchy = window.productHierarchy;

        record.data.ProductTreeObjectIds = '';
        window.productTreeNodes.forEach(function (objectId, index) {
            record.data.ProductTreeObjectIds += objectId;

            if (index != window.productTreeNodes.length - 1)
                record.data.ProductTreeObjectIds += ';';
        });
        record.data.InOutProductIds = window.InOutProductIds || record.data.InOutProductIds;
        record.data.InOut = (record.data.InOut ? true : false) || (window.isInOutPromo ? true : false);
        record.data.InOutExcludeAssortmentMatrixProductsButtonPressed = window.excludeAssortmentMatrixProductsButtonPressed ? true : false;
        //record.data.RegularExcludedProductIds = window.RegularExcludedProductIds;

        record.data.PromoTypesId = window.promotypeId || record.data.PromoTypesId;
        record.data.PromoTypesName = window.promotypeName || record.data.PromoTypesName;
        record.data.PromoTypesGlyph = window.promotypeGlyph || record.data.PromoTypesGlyph;


        record.data.IsGrowthAcceleration = window.isGrowthAcceleration ? true : false;

        record.data.IsApolloExport = window.isApolloExport ? true : false;

        record.data.Name = window.promoName;

        // promomechanic
        var marsMechanicId = promomechanic.down('searchcombobox[name=MarsMechanicId]').getValue();
        var marsMechanicTypeId = promomechanic.down('searchcombobox[name=MarsMechanicTypeId]').getValue();
        var marsMechanicDiscount = promomechanic.down('numberfield[name=MarsMechanicDiscount]').getValue();

        var instoreMechanicId = promomechanic.down('searchcombobox[name=PlanInstoreMechanicId]').getValue();
        var instoreMechanicTypeId = promomechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]').getValue();
        var instoreMechanicDiscount = promomechanic.down('numberfield[name=PlanInstoreMechanicDiscount]').getValue();

        record.data.MechanicComment = promomechanic.down('textarea[name=PromoComment]').getValue();

        record.data.MarsMechanicId = marsMechanicId ? marsMechanicId : null;
        record.data.MarsMechanicTypeId = marsMechanicTypeId ? marsMechanicTypeId : null;
        record.data.MarsMechanicDiscount = marsMechanicDiscount != null ? marsMechanicDiscount : null;

        record.data.PlanInstoreMechanicId = instoreMechanicId ? instoreMechanicId : null;
        record.data.PlanInstoreMechanicTypeId = instoreMechanicTypeId ? instoreMechanicTypeId : null;
        record.data.PlanInstoreMechanicDiscount = instoreMechanicDiscount != null ? instoreMechanicDiscount : null;

        // promoperiod
        record.data.StartDate = promoperiod.down('datefield[name=DurationStartDate]').getValue();
        record.data.EndDate = promoperiod.down('datefield[name=DurationEndDate]').getValue();
        record.data.DispatchesStart = promoperiod.down('datefield[name=DispatchStartDate]').getValue();
        record.data.DispatchesEnd = promoperiod.down('datefield[name=DispatchEndDate]').getValue();

        //promo budget year
        record.data.BudgetYear = promobudgetyear.down('combobox').getValue();

        // promoevent
        record.data.EventId = promoevent.down('chooseEventButton').getValue();

        // promosettings
        record.data.CalendarPriority = promosettings.down('sliderfield[name=priority]').getValue();

        // promoadjustment
        record.data.DeviationCoefficient = -promoadjustment.down('sliderfield[name=DeviationCoefficient]').getValue();

        // --------------- promo budgets ---------------
        var totalCostBudgets = promoBudgets.down('container[name=promoBudgets_step1]');
        var raTIShopper = promoBudgets.down('container[name=promoBudgets_step4]');

        // cost and budget
        record.data.PlanPromoTIShopper = totalCostBudgets.down('numberfield[name=PlanPromoTIShopper]').getValue();
        record.data.PlanPromoTIMarketing = totalCostBudgets.down('numberfield[name=PlanPromoTIMarketing]').getValue();
        record.data.PlanPromoBranding = totalCostBudgets.down('numberfield[name=PlanPromoBranding]').getValue();
        record.data.PlanPromoCost = totalCostBudgets.down('numberfield[name=PlanPromoCost]').getValue();
        record.data.PlanPromoBTL = totalCostBudgets.down('numberfield[name=PlanPromoBTL]').getValue();
        record.data.PlanPromoCostProduction = totalCostBudgets.down('numberfield[name=PlanPromoCostProduction]').getValue();

        record.data.ActualPromoTIShopper = totalCostBudgets.down('numberfield[name=ActualPromoTIShopper]').getValue();
        record.data.ActualPromoTIMarketing = totalCostBudgets.down('numberfield[name=ActualPromoTIMarketing]').getValue();
        record.data.ActualPromoBranding = totalCostBudgets.down('numberfield[name=ActualPromoBranding]').getValue();
        record.data.ActualPromoCost = totalCostBudgets.down('numberfield[name=ActualPromoCost]').getValue();
        record.data.ActualPromoBTL = totalCostBudgets.down('numberfield[name=ActualPromoBTL]').getValue();
        record.data.ActualPromoCostProduction = totalCostBudgets.down('numberfield[name=ActualPromoCostProduction]').getValue();

        record.data.PlanAddTIMarketingApproved = raTIShopper.down('numberfield[name=PlanAddTIMarketingApproved]').getValue();
        // --------------- calculation ---------------

        //var activity = promocalculation.down('container[name=activity]');
        //var financialIndicator = promocalculation.down('container[name=financialIndicator]');

        // activity
        //record.data.PlanPromoUpliftPercent = activity.down('triggerfielddetails[name=PlanPromoUpliftPercent]').getValue();
        //record.data.PlanPromoIncrementalLSV = activity.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoIncrementalLSV = activity.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoPostPromoEffectLSV = activity.down('trigger[name=PlanPostPromoEffectTotal]').getValue();

        //record.data.ActualPromoUpliftPercent = activity.down('numberfield[name=ActualPromoUpliftPercent]').getValue();
        //record.data.ActualPromoIncrementalLSV = activity.down('numberfield[name=ActualPromoIncrementalLSV]').getValue();
        //record.data.ActualPromoLSV = activity.down('numberfield[name=ActualPromoLSV]').getValue();
        //record.data.ActualPromoPostPromoEffectLSV = activity.down('trigger[name=FactPostPromoEffectTotal]').getValue();

        //// financial indicator
        //record.data.PlanPromoROIPercent = financialIndicator.down('numberfield[name=PlanPromoROIPercent]').getValue();
        //record.data.PlanPromoIncrementalNSV = financialIndicator.down('numberfield[name=PlanPromoIncrementalNSV]').getValue();
        //record.data.PlanPromoNetIncrementalNSV = financialIndicator.down('numberfield[name=PlanPromoNetIncrementalNSV]').getValue();
        //record.data.PlanPromoIncrementalMAC = financialIndicator.down('numberfield[name=PlanPromoIncrementalMAC]').getValue();

        //record.data.ActualPromoROIPercent = financialIndicator.down('numberfield[name=ActualPromoROIPercent]').getValue();
        //record.data.ActualPromoIncrementalNSV = financialIndicator.down('numberfield[name=ActualPromoIncrementalNSV]').getValue();
        //record.data.ActualPromoNetIncrementalNSV = financialIndicator.down('numberfield[name=ActualPromoNetIncrementalNSV]').getValue();
        //record.data.ActualPromoIncrementalMAC = financialIndicator.down('numberfield[name=ActualPromoIncrementalMAC]').getValue();

        // --------------- promo activity ---------------

        var promoActivityStep1 = promoActivity.down('container[name=promoActivity_step1]');
        var promoActivityStep2 = promoActivity.down('container[name=promoActivity_step2]');

        record.data.ActualInStoreMechanicId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicId]').getValue();
        record.data.ActualInStoreMechanicTypeId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicTypeId]').getValue();
        record.data.ActualInStoreDiscount = promoActivityStep1.down('numberfield[name=ActualInStoreDiscount]').getValue();

        record.data.ActualInStoreShelfPrice = promoActivityStep1.down('numberfield[name=ActualInStoreShelfPrice]').getValue();
        record.data.PlanInStoreShelfPrice = promoActivityStep1.down('numberfield[name=PlanInStoreShelfPrice]').getValue();

        record.data.InvoiceNumber = promoActivityStep2.down('textfield[name=InvoiceNumber]').getValue();
        record.data.SumInvoice = promoActivityStep2.down('triggerfielddetails[name=SumInvoice]').getValue();
        record.data.ManualInputSumInvoice = promoActivityStep2.down('#ManualInputSumInvoiceCheckbox').getValue();

        record.data.DocumentNumber = promoActivityStep2.down('textfield[name=DocumentNumber]').getValue();
        record.data.PlanPromoUpliftPercent = promoActivityStep2.down('triggerfielddetails[name=PlanPromoUpliftPercent]').getValue();

        var needRecountUplift = promoActivityStep2.down('#PromoUpliftLockedUpdateCheckbox').getValue();
        if (needRecountUplift === true) {
            record.data.NeedRecountUplift = false;
        } else {
            record.data.NeedRecountUplift = true;
        }
        //record.data.PlanPromoBaselineLSV = promoActivityStep2.down('numberfield[name=PlanPromoBaselineLSV]').getValue();
        //record.data.PlanPromoIncrementalLSV = promoActivityStep2.down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        //record.data.PlanPromoLSV = promoActivityStep2.down('numberfield[name=PlanPromoLSV]').getValue();
        //record.data.PlanPostPromoEffectTotal = promoActivityStep2.down('numberfield[name=PlanPostPromoEffectTotal]').getValue();

        //record.data.ActualPromoUpliftPercent = promoActivityStep2.down('numberfield[name=ActualPromoUpliftPercent]').getValue();
        //record.data.ActualPromoBaselineLSV = promoActivityStep2.down('numberfield[name=ActualPromoBaselineLSV]').getValue();
        //record.data.ActualPromoIncrementalLSV = promoActivityStep2.down('numberfield[name=ActualPromoIncrementalLSV]').getValue();
        //record.data.ActualPromoLSV = promoActivityStep2.down('numberfield[name=ActualPromoLSVByCompensation]').getValue();
        //record.data.FactPostPromoEffectTotal = promoActivityStep2.down('numberfield[name=FactPostPromoEffectTotal]').getValue();
        //if (App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator') {
        //    //Только для SupportAdmin
        //    record.data.PlanPromoBaselineLSV = promoActivityStep2.down('triggerfielddetails[name=PlanPromoBaselineLSV]').getValue();
        //    record.data.PlanPromoIncrementalLSV = promoActivityStep2.down('triggerfielddetails[name=PlanPromoIncrementalLSV]').getValue();
        //    record.data.PlanPromoLSV = promoActivityStep2.down('triggerfielddetails[name=PlanPromoLSV]').getValue();
        //    record.data.PlanPromoPostPromoEffectLSV = promoActivityStep2.down('triggerfielddetails[name=PlanPromoPostPromoEffectLSV]').getValue();
        //    record.data.ActualPromoUpliftPercent = promoActivityStep2.down('triggerfielddetails[name=ActualPromoUpliftPercent]').getValue();
        //    record.data.ActualPromoBaselineLSV = promoActivityStep2.down('triggerfielddetails[name=ActualPromoBaselineLSV]').getValue();
        //    record.data.ActualPromoIncrementalLSV = promoActivityStep2.down('triggerfielddetails[name=ActualPromoIncrementalLSV]').getValue();
        //    record.data.ActualPromoPostPromoEffectLSV = promoActivityStep2.down('triggerfielddetails[name=ActualPromoPostPromoEffectLSV]').getValue();
        //    record.data.ActualPromoLSV = promoActivityStep2.down('triggerfielddetails[name=ActualPromoLSV]').getValue();
        //    record.data.ActualPromoLSVByCompensation = promoActivityStep2.down('triggerfielddetails[name=ActualPromoLSVByCompensation]').getValue();

        //    record.data.PlanPromoCost = promoBudgets.down('numberfield[name=PlanPromoCost]').getValue();
        //    record.data.PlanPromoTIMarketing = promoBudgets.down('numberfield[name=PlanPromoTIMarketing]').getValue();
        //    record.data.PlanPromoCostProduction = promoBudgets.down('numberfield[name=PlanPromoCostProduction]').getValue();
        //    record.data.PlanPromoTIShopper = promoBudgets.down('numberfield[name=PlanPromoTIShopper]').getValue();
        //    record.data.ActualPromoCost = promoBudgets.down('numberfield[name=ActualPromoCost]').getValue();
        //    record.data.ActualPromoTIMarketing = promoBudgets.down('numberfield[name=ActualPromoTIMarketing]').getValue();
        //    record.data.ActualPromoCostProduction = promoBudgets.down('numberfield[name=ActualPromoCostProduction]').getValue();
        //    record.data.ActualPromoTIShopper = promoBudgets.down('numberfield[name=ActualPromoTIShopper]').getValue();

        //    var marketingTIStep = promoBudgets.down('container[name=promoBudgets_step2]');

        //    record.data.PlanPromoXSites = marketingTIStep.down('triggerfield[name=budgetDet-PlanX-sites]').getValue();
        //    record.data.PlanPromoCatalogue = marketingTIStep.down('triggerfield[name=budgetDet-PlanCatalog]').getValue();
        //    record.data.PlanPromoPOSMInClient = marketingTIStep.down('triggerfield[name=budgetDet-PlanPOSM]').getValue();

        //    record.data.ActualPromoXSites = marketingTIStep.down('triggerfield[name=budgetDet-ActualX-sites]').getValue();
        //    record.data.ActualPromoCatalogue = marketingTIStep.down('triggerfield[name=budgetDet-ActualCatalog]').getValue();
        //    record.data.ActualPromoPOSMInClient = marketingTIStep.down('triggerfield[name=budgetDet-ActualPOSM]').getValue();

        //    // cost production
        //    var costProductionStep = promoBudgets.down('container[name=promoBudgets_step3]');

        //    record.data.PlanPromoCostProdXSites = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdX-sites]').getValue();
        //    record.data.PlanPromoCostProdCatalogue = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdCatalog]').getValue();
        //    record.data.PlanPromoCostProdPOSMInClient = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdPOSM]').getValue();

        //    record.data.ActualPromoCostProdXSites = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdX-sites]').getValue();
        //    record.data.ActualPromoCostProdCatalogue = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdCatalog]').getValue();
        //    record.data.ActualPromoCostProdPOSMInClient = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdPOSM]').getValue();

        //}

        return record;
    },

    getBaseClientTreeId: function (treegrid, item) {
        if (item.get('IsBaseClient')) {
            return item.get('ObjectId')
        } else {
            // Идем наверх по дереву, пока не найдем базового клиента
            var store = treegrid.getStore(),
                parent = store.getNodeById(item.get('parentId'));
            if (parent) {
                while (parent.data.root !== true) {
                    if (parent.get('IsBaseClient')) {
                        return parent.get('ObjectId')
                    } else {
                        parent = store.getNodeById(parent.get('parentId'));
                    }
                }
            }
        }
        return null;
    },

    fillPromoForm: function (promoeditorcustom, record, readOnly, isCopy, promoGrid) {
        var me = this;
        // для блокировки/разблокировки грида/календаря
        // Если кнопка не null, то из грида, иначе из календаря
        var parentWidget = promoGrid ? promoGrid.up('promo') : Ext.ComponentQuery.query('schedulecontainer')[0];
        parentWidget.setLoading(true);
        // т.к. при открытии промо больше нет ожидания загрузки статусов, нужно сначала повесить лоадер
        Ext.Function.defer(me.fillPromoFormAfterSetLoading, 1, me, [promoeditorcustom, record, readOnly, isCopy, parentWidget]);
    },

    //вызывается отложенно из fillPromoForm для корректной отрисовки лоадера
    fillPromoFormAfterSetLoading: function (promoeditorcustom, record, readOnly, isCopy, parentWidget) {
        Ext.suspendLayouts();
        var me = this,
            calculating = isCopy ? false : record.get('Calculating');
        // Кнопки для изменения состояний промо
        var promoActions = Ext.ComponentQuery.query('button[isPromoAction=true]');
        var mechanic = promoeditorcustom.down('container[name=promo_step3]');

        // Для InOut Promo
        promoeditorcustom.isInOutPromo = record.data.InOut;

        promoeditorcustom.promotypeId = record.data.PromoTypesId;
        promoeditorcustom.promotypeName = record.data.PromoTypesName;
        promoeditorcustom.promotypeGlyph = record.data.PromoTypesGlyph;
        this.setPromoType(record.data.PromoTypesName, promoeditorcustom);
        //Промо в статусе Cancelled нельзя менять
        if (record.data.PromoStatusSystemName == 'Cancelled') {
            readOnly = true;
        } else {
            readOnly = isCopy ? false : readOnly || calculating;
        };
        promoeditorcustom.isInOutPromo = record.data.InOut;

        // Для Growth Acceleration Promo
        promoeditorcustom.isGrowthAcceleration = record.data.IsGrowthAcceleration;
        var growthAccelerationCheckbox = promoeditorcustom.down('[name=GrowthAccelerationCheckbox]');
        growthAccelerationCheckbox.setValue(record.data.IsGrowthAcceleration);

        // Для Apollo Export Promo
        promoeditorcustom.isApolloExport = record.data.IsApolloExport;
        var apolloExportCheckbox = promoeditorcustom.down('[name=ApolloExportCheckbox]');
        apolloExportCheckbox.setValue(record.data.IsApolloExport);

        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var gaReadOnlyStatuses = ['Approved', 'Planned', 'Started', 'Finished'];
        if (gaReadOnlyStatuses.indexOf(record.data.PromoStatusSystemName) != -1 && currentRole !== 'SupportAdministrator') {
            growthAccelerationCheckbox.setReadOnly(true);
        }

        promoeditorcustom.readOnly = readOnly;
        $.ajax({
            dataType: 'json',
            url: '/odata/PromoStatuss',
            success: function (promoStatusData) {
                if (!promoeditorcustom.isDestroyed) {
                    var draftStatusData = null;
                    for (var i = 0; i < promoStatusData.value.length; i++) {
                        if (promoStatusData.value[i].Id == record.data.PromoStatusId) {
                            promoeditorcustom.statusId = promoStatusData.value[i].Id;
                            promoeditorcustom.promoStatusName = promoStatusData.value[i].Name;
                            promoeditorcustom.promoStatusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'DraftPublished') {
                            var btn_publish = promoeditorcustom.down('button[itemId=btn_publish]');
                            btn_publish.statusId = promoStatusData.value[i].Id;
                            btn_publish.statusName = promoStatusData.value[i].Name;
                            btn_publish.statusSystemName = promoStatusData.value[i].SystemName;
                            var btn_backToDraftPublished = promoeditorcustom.down('button[itemId=btn_backToDraftPublished]');
                            btn_backToDraftPublished.statusId = promoStatusData.value[i].Id;
                            btn_backToDraftPublished.statusName = promoStatusData.value[i].Name;
                            btn_backToDraftPublished.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'OnApproval') {
                            var btn_sendForApproval = promoeditorcustom.down('button[itemId=btn_sendForApproval]');
                            btn_sendForApproval.statusId = promoStatusData.value[i].Id;
                            btn_sendForApproval.statusName = promoStatusData.value[i].Name;
                            btn_sendForApproval.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Approved') {
                            var btn_approve = promoeditorcustom.down('button[itemId=btn_approve]');
                            btn_approve.statusId = promoStatusData.value[i].Id;
                            btn_approve.statusName = promoStatusData.value[i].Name;
                            btn_approve.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Cancelled') {
                            var btn_cancel = promoeditorcustom.down('button[itemId=btn_cancel]');
                            btn_cancel.statusId = promoStatusData.value[i].Id;
                            btn_cancel.statusName = promoStatusData.value[i].Name;
                            btn_cancel.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Planned') {
                            var btn_plan = promoeditorcustom.down('button[itemId=btn_plan]');
                            btn_plan.statusId = promoStatusData.value[i].Id;
                            btn_plan.statusName = promoStatusData.value[i].Name;
                            btn_plan.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Closed') {
                            var btn_close = promoeditorcustom.down('button[itemId=btn_close]');
                            btn_close.statusId = promoStatusData.value[i].Id;
                            btn_close.statusName = promoStatusData.value[i].Name;
                            btn_close.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Finished') {
                            var btn_backToFinished = promoeditorcustom.down('button[itemId=btn_backToFinished]');
                            btn_backToFinished.statusId = promoStatusData.value[i].Id;
                            btn_backToFinished.statusName = promoStatusData.value[i].Name;
                            btn_backToFinished.statusSystemName = promoStatusData.value[i].SystemName;
                        }

                        if (promoStatusData.value[i].SystemName == 'Draft') {
                            draftStatusData = promoStatusData.value[i];

                            var undoBtn = promoeditorcustom.down('button[itemId=btn_undoPublish]');
                            undoBtn.statusId = promoStatusData.value[i].Id;
                            undoBtn.statusName = promoStatusData.value[i].Name;
                            undoBtn.statusSystemName = promoStatusData.value[i].SystemName;
                        }
                    }
                    if (isCopy) {
                        promoeditorcustom.statusId = draftStatusData.Id;
                        promoeditorcustom.promoStatusName = draftStatusData.Name;
                        promoeditorcustom.promoStatusSystemName = draftStatusData.SystemName;
                        me.setPromoTitle(promoeditorcustom, promoeditorcustom.promoName, promoeditorcustom.promoStatusName);
                        me.defineAllowedActions(promoeditorcustom, promoActions, promoeditorcustom.promoStatusName);
                    }
                    me.updateStatusHistoryState();
                }
            },
            error: function () {
                parentWidget.setLoading(false);
                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
            }
        });

        var customtoptoolbar = promoeditorcustom.down('customtoptoolbar');
        var promoController = App.app.getController('tpm.promo.Promo');

        // --------------- basic promo ---------------
        var promoClientForm = promoeditorcustom.down('container[name=promo_step1]');
        var promoProductForm = promoeditorcustom.down('container[name=promo_step2]');

        var period = promoeditorcustom.down('container[name=promo_step4]');
        var budgetYear = promoeditorcustom.down('container[name=promo_step5]');
        var event = promoeditorcustom.down('container[name=promo_step6]');
        var settings = promoeditorcustom.down('container[name=promo_step7]');
        var adjustment = promoeditorcustom.down('container[name=promo_step8]');

        // InOut Products
        promoeditorcustom.InOutProductIds = record.data.InOutProductIds;
        // Regular Products
        //promoeditorcustom.RegularExcludedProductIds = record.data.RegularExcludedProductIds;

        // mechanic
        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');
        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');
        var promoComment = mechanic.down('textarea[name=PromoComment]');

        // period
        var durationStartDate = period.down('datefield[name=DurationStartDate]');
        var durationEndDate = period.down('datefield[name=DurationEndDate]');
        var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
        var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');

        //budget year
        var budgetYearCombo = budgetYear.down('combobox');

        // event
        var promoEvent = event.down('chooseEventButton');

        //settings
        var priority = settings.down('sliderfield[name=priority]');

        // --------------- promo budgets ---------------

        var promoBudgets = promoeditorcustom.down('promobudgets');

        // total cost budgets
        var totalCostBudgets = promoBudgets.down('container[name=promoBudgets_step1]');

        var shopperTi = totalCostBudgets.down('numberfield[name=PlanPromoTIShopper]');
        var marketingTi = totalCostBudgets.down('numberfield[name=PlanPromoTIMarketing]');
        var branding = totalCostBudgets.down('numberfield[name=PlanPromoBranding]');
        var totalCost = totalCostBudgets.down('numberfield[name=PlanPromoCost]');
        var btl = totalCostBudgets.down('numberfield[name=PlanPromoBTL]');
        var costProduction = totalCostBudgets.down('numberfield[name=PlanPromoCostProduction]');

        var actualPromoTIShopper = totalCostBudgets.down('numberfield[name=ActualPromoTIShopper]');
        var actualPromoTIMarketing = totalCostBudgets.down('numberfield[name=ActualPromoTIMarketing]');
        var actualPromoBranding = totalCostBudgets.down('numberfield[name=ActualPromoBranding]');
        var factTotalCost = totalCostBudgets.down('numberfield[name=ActualPromoCost]');
        var factBtl = totalCostBudgets.down('numberfield[name=ActualPromoBTL]');
        var factCostProduction = totalCostBudgets.down('numberfield[name=ActualPromoCostProduction]');

        // marketing TI
        var marketingTIStep = promoBudgets.down('container[name=promoBudgets_step2]');

        var planPromoXSites = marketingTIStep.down('triggerfield[name=budgetDet-PlanX-sites]');
        var planPromoCatalogue = marketingTIStep.down('triggerfield[name=budgetDet-PlanCatalog]');
        var planPromoPOSMInClient = marketingTIStep.down('triggerfield[name=budgetDet-PlanPOSM]');

        var actualPromoXSites = marketingTIStep.down('triggerfield[name=budgetDet-ActualX-sites]');
        var actualPromoCatalogue = marketingTIStep.down('triggerfield[name=budgetDet-ActualCatalog]');
        var actualPromoPOSMInClient = marketingTIStep.down('triggerfield[name=budgetDet-ActualPOSM]');

        // cost production
        var costProductionStep = promoBudgets.down('container[name=promoBudgets_step3]');

        var planPromoCostProdXSites = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdX-sites]');
        var planPromoCostProdCatalogue = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdCatalog]');
        var planPromoCostProdPOSMInClient = costProductionStep.down('triggerfield[name=budgetDet-PlanCostProdPOSM]');

        var actualPromoCostProdXSites = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdX-sites]');
        var actualPromoCostProdCatalogue = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdCatalog]');
        var actualPromoCostProdPOSMInClient = costProductionStep.down('triggerfield[name=budgetDet-ActualCostProdPOSM]');

        // add ti
        var addTIStep = promoBudgets.down('container[name=promoBudgets_step4]');

        var planAddTIShopperApproved = addTIStep.down('[name=PlanAddTIShopperApproved]');
        var planAddTIShopperCalculated = addTIStep.down('[name=PlanAddTIShopperCalculated]');
        var planAddTIMarketingApproved = addTIStep.down('[name=PlanAddTIMarketingApproved]');
        var actualAddTIShopper = addTIStep.down('[name=ActualAddTIShopper]');
        var actualAddTIMarketing = addTIStep.down('[name=ActualAddTIMarketing]');

        // --------------- promo activity ---------------

        var promoActivity = promoeditorcustom.down('promoactivity');
        var promoActivityStep1 = promoActivity.down('container[name=promoActivity_step1]');
        var promoActivityStep2 = promoActivity.down('container[name=promoActivity_step2]');

        var actualInstoreMechanicId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actualInstoreMechanicTypeId = promoActivityStep1.down('searchcombobox[name=ActualInstoreMechanicTypeId]');
        var actualInStoreDiscount = promoActivityStep1.down('numberfield[name=ActualInStoreDiscount]');

        var actualInStoreShelfPrice = promoActivityStep1.down('numberfield[name=ActualInStoreShelfPrice]');
        var planInStoreShelfPrice = promoActivityStep1.down('numberfield[name=PlanInStoreShelfPrice]');
        var SumInvoice = promoActivityStep2.down('triggerfielddetails[name=SumInvoice]');
        var manualInputSumInvoiceCheckbox = promoActivityStep2.down('checkbox[itemId=ManualInputSumInvoiceCheckbox]');
        var invoiceNumber = promoActivityStep2.down('textfield[name=InvoiceNumber]');
        var documentNumber = promoActivityStep2.down('textfield[name=DocumentNumber]');
        var planPromoUpliftPercent = promoActivityStep2.down('[name=PlanPromoUpliftPercent]');
        var promoUpliftLockedUpdateCheckbox = promoActivityStep2.down('checkbox[itemId=PromoUpliftLockedUpdateCheckbox]');
        var planPromoBaselineLSV = promoActivityStep2.down('[name=PlanPromoBaselineLSV]');
        var planPromoIncrementalLSV = promoActivityStep2.down('[name=PlanPromoIncrementalLSV]');
        var planPromoLSV = promoActivityStep2.down('[name=PlanPromoLSV]');
        var planPostPromoEffect = promoActivityStep2.down('[name=PlanPromoPostPromoEffectLSV]');

        var actualPromoUpliftPercent = promoActivityStep2.down('[name=ActualPromoUpliftPercent]');
        var actualPromoBaselineLSV = promoActivityStep2.down('[name=ActualPromoBaselineLSV]');
        var actualPromoIncrementalLSV = promoActivityStep2.down('[name=ActualPromoIncrementalLSV]');
        var actualPromoLSV = promoActivityStep2.down('[name=ActualPromoLSV]');
        var actualPromoLSVSO = promoActivityStep2.down('[name=ActualPromoLSVSO]');
        var actualPromoLSVbyCompensation = promoActivityStep2.down('[name=ActualPromoLSVByCompensation]');
        var factPostPromoEffect = promoActivityStep2.down('[name=ActualPromoPostPromoEffectLSV]');

        // Нельзя загружать актуальные параметры до окончания промо.
        //var uploadActualsButton = promoActivityStep2.down('#activityUploadPromoProducts');
        //if (uploadActualsButton && record.data && record.data.PromoStatusSystemName == 'Finished') {
        //	uploadActualsButton.setDisabled(false);
        //} else {
        //	uploadActualsButton.setDisabled(true);
        //}

        me.setReadOnlyForChildrens(promoActivity, record.data.PromoStatusSystemName, !readOnly, promoeditorcustom.isInOutPromo);
        me.setReadOnlyForChildrens(promoBudgets, record.data.PromoStatusSystemName, !readOnly, promoeditorcustom.isInOutPromo);


        // Блокировка изменения значений
        if (readOnly) {
            // --------------- basic promo ---------------
            // client
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);

            // product
            promoProductForm.setDisabledBtns(true);

            // event
            promoEvent.setDisabled(true); // button
            budgetYearCombo.addCls('readOnlyField');

            //budget year
            budgetYearCombo.setReadOnly(true);

            // settings
            priority.setReadOnly(true);

            var elementsToReadOnly = promoeditorcustom.query('[needReadOnly=true]');
            me.setReadOnlyProperty(true, elementsToReadOnly);

            // --------------- budgets promo ---------------
            // блокировка кнопки Add Promo Support в режиме просмотра.
            //var addSubItemButtons = promoBudgets.query('#addSubItem');
            //addSubItemButtons.forEach(function (button) {
            //    button.setDisabled(true);
            //})

            // --------------- promo activity ---------------
            //promoeditorcustom.down('container[itemId=ContainerPlanPromoUplift]').setReadable(true);

            // --------------- buttons ---------------
            promoeditorcustom.down('button[itemId=savePromo]').hide();
            promoeditorcustom.down('button[itemId=saveAndClosePromo]').hide();
            promoeditorcustom.down('button[itemId=cancelPromo]').hide();
            promoeditorcustom.down('button[itemId=closePromo]').show();
            if (record && record.data && record.data.PromoStatusSystemName === 'Cancelled') {
                promoeditorcustom.down('button[itemId=changePromo]').hide();
            } else {
                promoeditorcustom.down('button[itemId=changePromo]').show();
            }
        }

        if (record.data.PromoStatusSystemName === 'Closed') {
            //budgetYear.setDisabled(true);
            budgetYearCombo.setReadOnly(true);
            budgetYearCombo.addCls('readOnlyField');
        }

        if (isCopy) {
            promoeditorcustom.promoName = 'Unpublish Promo';
        }

        if (App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator') {
            promoeditorcustom.promoId = record.data.Id;
            promoeditorcustom.promoNumber = record.data.Number;
            promoeditorcustom.statusId = record.data.PromoStatusId;
            promoeditorcustom.promoName = record.data.Name;

            promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
            promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
            promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
            promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
        } else {
            if (isCopy) {
                promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
                promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
            } else {
                promoeditorcustom.promoId = record.data.Id;
                promoeditorcustom.promoNumber = record.data.Number;
                promoeditorcustom.statusId = record.data.PromoStatusId;
                promoeditorcustom.promoName = record.data.Name;

                switch (record.data.PromoStatusSystemName) {
                    case 'Draft':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
                        break;
                    case 'DraftPublished':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    case 'OnApproval':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    case 'Approved':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    case 'Planned':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    case 'Started':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    case 'Finished':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    case 'Closed':
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').removeCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(false);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').removeCls('disabled');
                        break;
                    default:
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').setDisabled(true);
                        promoeditorcustom.down('button[itemId=btn_promoBudgets]').addCls('disabled');
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').setDisabled(true);
                        promoeditorcustom.down('button[itemId=btn_promoActivity]').addCls('disabled');
                        break;
                }
            }
        }

        var titleText = isCopy ?
            Ext.String.format('Name: {0} (ID: {1}), Status: {2}', promoeditorcustom.promoName, promoeditorcustom.promoNumber, promoeditorcustom.promoStatusName) :
            Ext.String.format('Name: {0} (ID: {1}), Status: {2}', record.data.Name, record.data.Number, record.data.PromoStatusName);

        customtoptoolbar.down('label[name=promoName]').setText(titleText);

        // Деревья client и Product забираются по дате при статусе Approval для статусов после и при Approved
        var treesChangingBlockDate = ['Draft', 'DraftPublished', 'OnApproval'].indexOf(record.data.PromoStatusSystemName) < 0 ? record.get('LastApprovedDate') : null;
        if (treesChangingBlockDate) {
            treesChangingBlockDate = Ext.Date.format(treesChangingBlockDate, 'Y-m-d\\TH:i:s');
        }

        // client
        // Если запись создаётся копированием, клиент берётся из календаря, а не из копируемой записи        
        var clientRecord = isCopy ? record.schedulerContext.resourceRecord.raw : record.raw.ClientTree;

        if (clientRecord) {
            promoeditorcustom.model = isCopy ? promoeditorcustom.model : record;
            promoClientForm.fillForm(clientRecord, treesChangingBlockDate);
        }

        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var clientCrudAccess = ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];

        if (clientCrudAccess.indexOf(currentRole) > -1) {
            promoClientForm.down('#choosePromoClientBtn').setDisabled(readOnly);
        } else {
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);
        }

        // product
        promoProductForm.fillFormJson(record.data.PromoBasicProducts, treesChangingBlockDate, record);
        if (record.data.PromoBasicProducts)
            me.setInfoPromoBasicStep2(promoProductForm);

        var productCrudAccess = ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        if (productCrudAccess.indexOf(currentRole) > -1) {
            promoProductForm.setDisabledBtns(readOnly);
        } else {
            promoProductForm.setDisabledBtns(true);
        }

        // mechanic
        marsMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.MarsMechanicId,
            Name: record.data.MarsMechanicName
        }));
        if (record.data.MarsMechanicTypeId) {
            marsMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
                Id: record.data.MarsMechanicTypeId,
                Name: record.data.MarsMechanicTypeName
            }));
            marsMechanicDiscount.setValue(record.data.MarsMechanicDiscount);
        } else if (record.data.MarsMechanicDiscount != null) {
            marsMechanicDiscount.setValue(record.data.MarsMechanicDiscount);
            marsMechanicTypeId.clearInvalid();
        }

        if (marsMechanicId.crudAccess.indexOf(currentRole) === -1) {
            marsMechanicId.setReadOnly(true);
        }
        if (marsMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
            marsMechanicTypeId.setReadOnly(true);
        }
        if (marsMechanicDiscount.crudAccess.indexOf(currentRole) === -1) {
            marsMechanicDiscount.setReadOnly(true);
        }

        instoreMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.PlanInstoreMechanicId,
            Name: record.data.PlanInstoreMechanicName
        }));
        if (record.data.PlanInstoreMechanicTypeId) {
            instoreMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
                Id: record.data.PlanInstoreMechanicTypeId,
                Name: record.data.PlanInstoreMechanicTypeName
            }));
            instoreMechanicDiscount.setValue(record.data.PlanInstoreMechanicDiscount);
        } else if (record.data.PlanInstoreMechanicId && record.data.PlanInstoreMechanicDiscount != null) {
            instoreMechanicDiscount.setValue(record.data.PlanInstoreMechanicDiscount);
        }

        instoreMechanicTypeId.clearInvalid();
        instoreMechanicDiscount.clearInvalid();

        if (instoreMechanicId.crudAccess.indexOf(currentRole) === -1) {
            instoreMechanicId.setReadOnly(true);
        }
        if (instoreMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
            instoreMechanicTypeId.setReadOnly(true);
        }
        if (instoreMechanicDiscount.crudAccess.indexOf(currentRole) === -1) {
            instoreMechanicDiscount.setReadOnly(true);
        }

        promoController.mechanicTypeChange(
            marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            readOnly, true
        );

        promoController.mechanicTypeChange(
            instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
            promoController.getMechanicListForUnlockDiscountField(),
            readOnly, true
        );

        promoComment.setValue(record.data.MechanicComment);

        // period
        // Если запись создаётся копированием, даты берутся из календаря, а не из копируемой записи
        var startDate = isCopy ? record.schedulerContext.start : record.data.StartDate;
        var endDate = isCopy ? record.schedulerContext.end : record.data.EndDate;
        durationStartDate.setValue(startDate);
        durationEndDate.setValue(endDate);

        budgetYearCombo.setValue(record.data.BudgetYear);

        if (durationStartDate.crudAccess.indexOf(currentRole) === -1) {
            durationStartDate.setReadOnly(true);
        }
        if (durationEndDate.crudAccess.indexOf(currentRole) === -1) {
            durationEndDate.setReadOnly(true);
        }

        if (dispatchStartDate.crudAccess.indexOf(currentRole) === -1) {
            dispatchStartDate.setReadOnly(true);
        }
        if (dispatchEndDate.crudAccess.indexOf(currentRole) === -1) {
            dispatchEndDate.setReadOnly(true);
        }

        if (clientRecord) {
            me.checkParametersAfterChangeClient(clientRecord, promoeditorcustom);
            me.afterInitClient(clientRecord, record, promoeditorcustom, isCopy);
        }

        me.refreshPromoEvent(promoeditorcustom, false);

        if (promoEvent.crudAccess.indexOf(currentRole) === -1) {
            promoEvent.setDisabled(true);
        }

        var stateListForExcludePeriodValidation = ['Cancelled', 'Started', 'Finished', 'Closed', 'Deleted', 'Undefined'];
        stateListForExcludePeriodValidation.forEach(function (state) {
            if (state == record.data.PromoStatusSystemName) {
                durationStartDate.setMinValue(null);
                durationStartDate.setMaxValue(null);
                durationStartDate.clearInvalid();

                durationEndDate.setMinValue(null);
                durationEndDate.setMaxValue(null);
                durationEndDate.clearInvalid();
            }
        });

        // settings
        priorityValue = isCopy ? 3 : record.data.CalendarPriority;
        priority.setValue(priorityValue);

        var promoEventButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step7]')[0];
        promoEventButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep7') + '</b><br><p>' + l10n.ns('tpm', 'Promo').value('CalendarPriority') + ': ' + priorityValue + '</p>');
        promoEventButton.removeCls('notcompleted');
        promoEventButton.setGlyph(0xf133);

        // Если Промо в статусе от Started, то заблокировать редактирование PromoBasic
        var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(record.data.PromoStatusSystemName) >= 0);
        if (isPromoWasStarted && currentRole !== 'SupportAdministrator') {
            // --------------- basic promo ---------------
            // client
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);
            promoClientForm.down('[id=OnInvoice]').setReadOnly(true);
            promoClientForm.down('[id=OffInvoice]').setReadOnly(true);

            // product
            promoProductForm.setDisabledBtns(true);

            // event
            promoEvent.setDisabled(true); // button

            // settings
            priority.setReadOnly(true);

            var elementsToReadOnly = promoeditorcustom.down('container[itemId=basicPromoPanel]').query('[needReadOnly=true]');
            me.setReadOnlyProperty(true, elementsToReadOnly);

            promoController.mechanicTypeChange(
                marsMechanicId, marsMechanicTypeId, marsMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                true
            );

            promoController.mechanicTypeChange(
                instoreMechanicId, instoreMechanicTypeId, instoreMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                true
            );
        }

        // Заблокировать IsApolloExport для редактирования в указанных ниже статусах
        var isPromoEnd = (['Finished', 'Closed', 'Cancelled', 'Deleted'].indexOf(record.data.PromoStatusSystemName) >= 0);
        if (isPromoEnd) {
            promoeditorcustom.down('[name=ApolloExportCheckbox]').setReadOnly(true);
        }
        if (record.data.PromoStatusSystemName === 'Started'
            && !readOnly
            && promoeditorcustom.down('[name=ApolloExportCheckbox]').crudAccess.indexOf(App.UserInfo.getCurrentRole()['SystemName']) >= 0) {
            promoeditorcustom.down('[name=ApolloExportCheckbox]').setDisabled(false);
            promoeditorcustom.down('[name=ApolloExportCheckbox]').setReadOnly(false);
        }


        // Заблокировать Adjustment для редактирования в указанных ниже статусах
        var isPromoEnd = (['Finished', 'Closed', 'Cancelled', 'Deleted'].indexOf(record.data.PromoStatusSystemName) >= 0);
        if (isPromoEnd
            || promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').crudAccess.indexOf(App.UserInfo.getCurrentRole()['SystemName']) === -1) {
            promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').setDisabled(true);
            promoeditorcustom.down('numberfield[name=Adjustment]').setReadOnly(true);
            promoeditorcustom.down('numberfield[name=Adjustment]').addCls('readOnlyField');
        }
        if (((record.data.PromoStatusSystemName === 'Started'
            && promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').crudAccess.indexOf(App.UserInfo.getCurrentRole()['SystemName']) >= 0)
            || ((['Finished', 'Closed'].indexOf(record.data.PromoStatusSystemName) >= 0)
                && App.UserInfo.getCurrentRole()['SystemName'] === 'SupportAdministrator'))
            && !readOnly) {
            promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').setDisabled(false);
            promoeditorcustom.down('sliderfield[name = DeviationCoefficient]').setReadOnly(false);
            promoeditorcustom.down('numberfield[name=Adjustment]').setDisabled(false);
            promoeditorcustom.down('numberfield[name=Adjustment]').setReadOnly(false);
            promoeditorcustom.down('numberfield[name=Adjustment]').removeCls('readOnlyField');
        }

        // Заблокировать Add TI не для GA
        if (!record.data.IsGrowthAcceleration) {
            var planPanel = Ext.ComponentQuery.query('#promoBudgets_step4_planpanel')[0];
            var actualPanel = Ext.ComponentQuery.query('#promoBudgets_step4_actualpanel')[0];
            var button = Ext.ComponentQuery.query('#btn_promoBudgets_step4')[0];
            planPanel.setDisabled(true);
            actualPanel.setDisabled(true);
            button.setDisabled(true);
            button.addCls('disabled');
        }

        //редактирование только если не approved
        //var isApproved = ['Approved', 'Planned', 'Started', 'Finished'].includes(promoeditorcustom.promoStatusSystemName);
        var isApproved = record.data.LastApprovedDate != null;
        if (!isApproved) {
            promoeditorcustom.down('panel[name=promoBudgets_step4]').down('numberfield[name=PlanAddTIMarketingApproved]').setReadOnly(readOnly);
        }

        if (!isCopy) {
            // --------------- promo budgets ---------------                

            // total cost budgets
            totalCost.setValue(record.data.PlanPromoCost || 0);
            shopperTi.setValue(record.data.PlanPromoTIShopper);
            marketingTi.setValue(record.data.PlanPromoTIMarketing);
            branding.setValue(record.data.PlanPromoBranding);
            btl.setValue(record.data.PlanPromoBTL);
            costProduction.setValue(record.data.PlanPromoCostProduction);

            factTotalCost.setValue(record.data.ActualPromoCost || 0);
            actualPromoTIShopper.setValue(record.data.ActualPromoTIShopper);
            actualPromoTIMarketing.setValue(record.data.ActualPromoTIMarketing);
            actualPromoBranding.setValue(record.data.ActualPromoBranding);
            factBtl.setValue(record.data.ActualPromoBTL);
            factCostProduction.setValue(record.data.ActualPromoCostProduction);

            // marketing TI               
            planPromoXSites.setValue(record.data.PlanPromoXSites);
            planPromoCatalogue.setValue(record.data.PlanPromoCatalogue);
            planPromoPOSMInClient.setValue(record.data.PlanPromoPOSMInClient);

            actualPromoXSites.setValue(record.data.ActualPromoXSites);
            actualPromoCatalogue.setValue(record.data.ActualPromoCatalogue);
            actualPromoPOSMInClient.setValue(record.data.ActualPromoPOSMInClient);

            // cost production               
            planPromoCostProdXSites.setValue(record.data.PlanPromoCostProdXSites);
            planPromoCostProdCatalogue.setValue(record.data.PlanPromoCostProdCatalogue);
            planPromoCostProdPOSMInClient.setValue(record.data.PlanPromoCostProdPOSMInClient);

            actualPromoCostProdXSites.setValue(record.data.ActualPromoCostProdXSites);
            actualPromoCostProdCatalogue.setValue(record.data.ActualPromoCostProdCatalogue);
            actualPromoCostProdPOSMInClient.setValue(record.data.ActualPromoCostProdPOSMInClient);

            // add ti

            planAddTIShopperApproved.setValue(record.data.PlanAddTIShopperApproved);
            planAddTIShopperCalculated.setValue(record.data.PlanAddTIShopperCalculated);
            planAddTIMarketingApproved.setValue(record.data.PlanAddTIMarketingApproved);
            actualAddTIShopper.setValue(record.data.ActualAddTIShopper);
            actualAddTIMarketing.setValue(record.data.ActualAddTIMarketing);
            // --------------- promo activity ---------------

            actualInstoreMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
                Id: record.data.ActualInStoreMechanicId,
                Name: record.data.ActualInStoreMechanicName
            }));
            if (record.data.ActualInStoreMechanicId) {
                actualInstoreMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
                    Id: record.data.ActualInStoreMechanicTypeId,
                    Name: record.data.ActualInStoreMechanicTypeName
                }));
                actualInStoreDiscount.setValue(record.data.ActualInStoreDiscount);
            } else if (record.data.ActualInStoreDiscount) {
                actualInStoreDiscount.setValue(record.data.ActualInStoreDiscount);
            }

            actualInstoreMechanicTypeId.clearInvalid();
            actualInStoreDiscount.clearInvalid();

            //if (actualInstoreMechanicId.crudAccess.indexOf(currentRole) === -1) {
            //    actualInstoreMechanicId.setReadOnly(true);
            //}
            //if (actualInstoreMechanicTypeId.crudAccess.indexOf(currentRole) === -1) {
            //    actualInstoreMechanicTypeId.setReadOnly(true);
            //}
            //if (actualInStoreDiscount.crudAccess.indexOf(currentRole) === -1) {
            //    actualInStoreDiscount.setReadOnly(true);
            //}

            promoController.mechanicTypeChange(
                actualInstoreMechanicId, actualInstoreMechanicTypeId, actualInStoreDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                readOnly, true
            );

            actualInStoreShelfPrice.setValue(record.data.ActualInStoreShelfPrice);
            planInStoreShelfPrice.setValue(record.data.PlanInStoreShelfPrice);
            SumInvoice.setValue(record.data.SumInvoice);
            manualInputSumInvoiceCheckbox.setValue(record.data.ManualInputSumInvoice);
            invoiceNumber.setValue(record.data.InvoiceNumber);
            documentNumber.setValue(record.data.DocumentNumber);

            //if (actualInStoreShelfPrice.crudAccess.indexOf(currentRole) === -1) {
            //    actualInStoreShelfPrice.setReadOnly(true);
            //}

            //if (invoiceNumber.crudAccess.indexOf(currentRole) === -1) {
            //    invoiceNumber.setReadOnly(true);
            //}

            //if (documentNumber.crudAccess.indexOf(currentRole) === -1) {
            //    documentNumber.setReadOnly(true);
            //}

            planPromoUpliftPercent.setValue(record.data.PlanPromoUpliftPercent);
            promoUpliftLockedUpdateCheckbox.setValue(!record.data.NeedRecountUplift);
            planPromoUpliftPercent.defaultValue = !record.data.NeedRecountUplift;

            //if (record.data.InOut) {
            //    planPromoUpliftPercent.changeEditable(false);
            //    promoUpliftLockedUpdateCheckbox.setDisabled(true);
            //    planPromoUpliftPercent.up('container').setReadable(true);
            //    planPromoUpliftPercent.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33f);
            //}

            //var promoStatusName = record.get('PromoStatusName');
            ////Начавшиеся promo не редактируются uplift
            //var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoStatusName) >= 0);
            //if (isPromoWasStarted) {
            //    me.blockStartedPromoUplift();
            //}

            planPromoBaselineLSV.setValue(record.data.PlanPromoBaselineLSV);
            planPromoIncrementalLSV.setValue(record.data.PlanPromoIncrementalLSV);
            planPromoLSV.setValue(record.data.PlanPromoLSV);
            planPostPromoEffect.setValue(record.data.PlanPromoPostPromoEffectLSV);

            actualPromoUpliftPercent.setValue(record.data.ActualPromoUpliftPercent);
            actualPromoBaselineLSV.setValue(record.data.ActualPromoBaselineLSV);
            actualPromoIncrementalLSV.setValue(record.data.ActualPromoIncrementalLSV);
            actualPromoLSVbyCompensation.setValue(record.data.ActualPromoLSVByCompensation);
            actualPromoLSV.setValue(record.data.ActualPromoLSV);
            actualPromoLSVSO.setValue(record.data.ActualPromoLSVSO);
            factPostPromoEffect.setValue(record.data.ActualPromoPostPromoEffectLSV);

            //Для Adjustment
            promoeditorcustom.deviationCoefficient = record.data.DeviationCoefficient === null ? 0 : record.data.DeviationCoefficient;
            var adjustmentSlider = promoeditorcustom.down('sliderfield[name = DeviationCoefficient]');
            var adjustmentNumber = promoeditorcustom.down('numberfield[name = Adjustment]');
            adjustmentSlider.setValue(-record.data.DeviationCoefficient);
            adjustmentNumber.setValue(record.data.DeviationCoefficient);

        }

        var promoComment = mechanic.down('textarea[name=PromoComment]');
        var pcReadOnlyStatuses = ['Closed', 'Deleted', 'Cancelled'];
        if (((pcReadOnlyStatuses.indexOf(record.data.PromoStatusSystemName) == -1 && promoComment.crudAccess.indexOf(currentRole) != -1)
            || currentRole == 'SupportAdministrator')
            && promoeditorcustom.readOnly == false) {
            promoComment.setReadOnly(false);
        }

        var promoAdjustmentButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step8]')[0];
        promoAdjustmentButton.removeCls('notcompleted');
        promoAdjustmentButton.setGlyph(0xf133);

        parentWidget.setLoading(false);

        // Определяем доступные действия
        me.defineAllowedActions(promoeditorcustom, promoActions, record.data.PromoStatusSystemName);

        //если производится расчет данного промо, то необходимо сделать соотвествующие визуальные изменения окна: 
        //цвет хедера меняется на красный, кнопка Редактировать - disable = true, появляется кнопка Show Log, заблокировать кнопки смены статусов
        var toolbar = promoeditorcustom.down('customtoptoolbar');
        //кнопки перехода статуса находятся не на customtoptoolbar, а на toolbar, при вызове down('toolbar') мы получаем customtoptoolbar, поэтому достаём через кнопку
        var toolbarbutton = promoeditorcustom.down('button[itemId=btn_sendForApproval]').up();
        if (calculating) {
            //planPromoUpliftPercent.up('container').setReadable(true);
            //planPromoUpliftPercent.changeEditable(false);
            toolbar.addCls('custom-top-panel-calculating');
            toolbarbutton.items.items.forEach(function (item, i, arr) {
                //  item.el.setStyle('backgroundColor', '#B53333');
                if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish', 'btn_sendForApproval', 'btn_reject', 'btn_backToDraftPublished', 'btn_approve', 'btn_cancel', 'btn_plan', 'btn_close', 'btn_backToFinished'].indexOf(item.itemId) > -1) {
                    item.setDisabled(true);
                }
            });

            var label = toolbar.down('label[name=promoName]');
            if (label) {
                label.setText(label.text + ' — Promo is blocked for recalculations');
            }

            promoeditorcustom.down('#changePromo').setDisabled(true);
            toolbar.down('#btn_showlog').addCls('showlog');
            //toolbar.down('#btn_showlog').show();
            toolbar.down('#btn_showlog').promoId = record.data.Id;
            promoeditorcustom.down('#btn_recalculatePromo').hide();
            promoeditorcustom.down('#btn_resetPromo').hide();

            //me.createTaskCheckCalculation(promoeditorcustom);
        }
        else if (record.data.PromoStatusSystemName == 'Draft' && App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'supportadministrator') {
            promoeditorcustom.down('#btn_recalculatePromo').hide();
            promoeditorcustom.down('#btn_resetPromo').show();
        } else if ((App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'administrator'
            || App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'demandplanning' || App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'supportadministrator')
            && record.data.PromoStatusSystemName != 'Draft' && record.data.PromoStatusSystemName != 'Cancelled') {
            promoeditorcustom.down('#btn_recalculatePromo').show();
            promoeditorcustom.down('#btn_resetPromo').hide();
        } else {
            promoeditorcustom.down('#btn_recalculatePromo').hide();
            promoeditorcustom.down('#btn_resetPromo').hide();
        }

        //чтобы убрать кнопку Recalculate при копировании промо в календаре
        if (isCopy) {
            promoeditorcustom.down('#btn_recalculatePromo').hide();
        }

        Ext.resumeLayouts(true);
        this.checkLogForErrors(record.getId());

        this.checkLoadingComponents();
        if (!promoeditorcustom.isVisible())
            promoeditorcustom.show();
        promoeditorcustom.setLoading(false);
    },

    saveModel: function (model, window, close, reloadPromo) {
        var grid = Ext.ComponentQuery.query('#promoGrid')[0];
        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
        var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var me = this;
        var store = null;
        if (grid) {
            store = grid.down('directorygrid').promoStore;
        }
        window.setLoading(l10n.ns('core').value('savingText'));

        // останавливаем подписку на статус до загрузки окна
        //if (reloadPromo)
        requestHub($.connection.logHub.server.unsubscribeStatus);
        promoeditorcustom.isChanged = true;

        //Если были правки Product Uplift, сначала фиксируем их
        if (promoeditorcustom.productUpliftChanged) {

            model.data.AdditionalUserTimestamp = promoeditorcustom.tempEditUpliftId;

            me.saveModelRequest(model, window, close, reloadPromo, grid, scheduler, me, store);

        } else {

            me.saveModelRequest(model, window, close, reloadPromo, grid, scheduler, me, store);
        }
    },

    saveModelRequest: function (model, window, close, reloadPromo, grid, scheduler, me, store) {
        // Response возвращается не полностью верный, mappings игнорируются
        model.save({
            success: function (response, req) {
                if (req.response.length > 0 && req.response[0].value && req.response[0].value.length > 0)
                    App.Notify.pushInfo(req.response[0].value);
                
                var wasCreating = window.isCreating;
                if (store) {
                    store.on({
                        single: true,
                        scope: this,
                        load: function (records, operation, success) {
                            model.set('Key');
                            //window.setLoading(false);
                            if (typeof grid.afterSaveCallback === 'function') {
                                grid.afterSaveCallback(grid);
                            }
                        }
                    });
                }
                if (close) {
                    window.setLoading(false);
                    window.close();
                } else {
                    me.currentPromoModel = model;
                    window.promoId = response.data.Id;
                    window.isCreating = false;
                    window.model = response;

                    // перезаполнить промо
                    if (reloadPromo) {
                        App.model.tpm.promo.Promo.load(response.data.Id, {
                            callback: function (newModel, operation) {
                                var directorygrid = grid ? grid.down('directorygrid') : null;
                                if (newModel) {
                                    var showLogBtn = window.down('#btn_showlog');
                                    if (showLogBtn) {
                                        showLogBtn.promoId = newModel.data.Id;
                                    }

                                    window.model = newModel;
                                    window.readOnly = true;
                                    me.reFillPromoForm(window, newModel, directorygrid);
                                }
                                else {
                                    var statusData = null;
                                    $.ajax({
                                        dataType: 'json',
                                        url: '/odata/PromoStatuss',
                                        success: function (promoStatuses) {
                                            if (window && !window.isDestroyed) {
                                                for (var i = 0; i < promoStatuses.value.length; i++) {
                                                    if (promoStatuses.value[i].SystemName == 'Draft') {
                                                        statusData = promoStatuses.value[i];
                                                        break;
                                                    }
                                                }

                                                if (statusData) {
                                                    response.data.PromoStatusId = statusData.Id;
                                                    response.data.PromoStatusName = statusData.Name;
                                                    response.data.PromoStatusSystemName = statusData.SystemName;
                                                    window.readOnly = true;

                                                    me.reFillPromoForm(window, response, directorygrid);
                                                }
                                                else {
                                                    window.setLoading(false);
                                                }
                                            }
                                        }
                                    });
                                }

                                // если было создано, то id был обновлен
                                if (wasCreating) {
                                    me.initSignalR(window);
                                }
                            }
                        });
                    }
                    else {
                        window.setLoading(false);
                    }
                }
                // Если создание из календаря - обновляем календарь
                if (scheduler) {
                    scheduler.resourceStore.reload();
                    scheduler.eventStore.reload();
                }

                return response;
            },
            failure: function () {
                // при неудаче возвращаем старый статус
                if (window.previousStatusId) {
                    window.statusId = window.previousStatusId;
                }

                model.reject();
                window.setLoading(false);

                return null;
            }
        });
    },

    savePromo: function (button, close, reloadForm) {
        var window = button.up('promoeditorcustom');

        var isModelComplete = this.validatePromoModel(window);

        if (isModelComplete === '') {
            var record = this.getRecord(window);

            if (window.promoStatusSystemName === 'Draft') {
                this.setPromoTitle(window, window.promoName, window.promoStatusName);
            } else {
                window.promoName = this.getPromoName(window);
                this.setPromoTitle(window, window.promoName, window.promoStatusName);
            }
            var model = this.buildPromoModel(window, record);
            this.saveModel(model, window, close, reloadForm);
        } else {
            App.Notify.pushInfo(isModelComplete);
        }
    },

    splitAndPublishPromo: function (button, controller) {
        var window = button.up('promoeditorcustom');
        var checkValid = controller.validatePromoModel(window);
        if (checkValid === '') {
            var record = controller.getRecord(window);

            window.previousStatusId = window.statusId;
            window.statusId = button.statusId;
            window.promoName = controller.getPromoName(window);

            var model = controller.buildPromoModel(window, record);
            model.data.Name = window.promoName;
            model.data.PromoStatusId = 'FE7FFE19-4754-E911-8BC8-08606E18DF3F';
            model.data.IsSplittable = true;
            controller.saveModel(model, window, true, true);
            controller.updateStatusHistoryState();
            App.Notify.pushInfo('Split of subranges completed successfully');

        } else {
            App.Notify.pushInfo(checkValid);
        }
    },

    defineAllowedActions: function (promoeditorcustom, promoActions, status) {
        var currentRole = App.UserInfo.getCurrentRole();

        for (var i = 0; i < promoActions.length; i++) {
            var roles = promoActions[i].roles;
            var statuses = promoActions[i].statuses;

            var _role = roles.find(function (element) { return element == currentRole['SystemName']; });
            var _status = statuses.find(function (element) { return element == status; });

            if (_role != null && _status != null) {
                var visible = true;

                if (_status == 'OnApproval' && promoeditorcustom.xtype == 'promoeditorcustom') {
                    var record = this.getRecord(promoeditorcustom);

                    switch (_role) {
                        case 'CMManager':
                            visible = !record.get('IsCMManagerApproved');
                            break;

                        case 'Administrator':
                            visible = true;
                            break;

                        case 'FunctionalExpert':
                            visible = true;
                            break;

                        case 'CustomerMarketing':
                            visible = true;
                            break;

                        case 'DemandPlanning':
                            visible = record.get('IsCMManagerApproved') && !record.get('IsDemandPlanningApproved');
                            break;

                        case 'DemandFinance':
                            visible = record.get('IsCMManagerApproved') && record.get('IsDemandPlanningApproved') && !record.get('IsDemandFinanceApproved');
                            break;

                        case 'KeyAccountManager':
                            //Теперь любой KAM может переносить в Draft(published), если подходит по constrains
                            visible = true;
                            break;

                        default: break;
                    }
                }

                promoActions[i].setVisible(visible);
            }
            else {
                promoActions[i].setVisible(false);
            }
        }
    },

    setPromoTitle: function (component, name, status) {
        var promoTitle = component.down('customtoptoolbar').down('label[name=promoName]');
        promoTitle.setText('Name: ' + name + ', Status: ' + status);
    },

    getPromoName: function (promoForm) {
        var promoProductForm = promoForm.down('promobasicproducts');
        var promoMechanic = promoForm.down('panel').down('promomechanic');

        var marsMechanic = promoMechanic.down('searchcombobox[name=MarsMechanicId]').getRawValue();
        var marsMechanicType = promoMechanic.down('searchcombobox[name=MarsMechanicTypeId]').getRawValue();
        var marsMechanicDiscountField = promoMechanic.down('[name=MarsMechanicDiscount]');
        var marsMechanicDiscountValue = marsMechanicDiscountField.getValue();
        var discountValue = marsMechanic !== 'VP' ? marsMechanicDiscountValue + '%' : marsMechanicType

        var promoName = (promoProductForm.brandAbbreviation ? promoProductForm.brandAbbreviation : '') + ' ' + (promoProductForm.technologyAbbreviation ? promoProductForm.technologyAbbreviation : '') + ' ' +
            marsMechanic + ' ' + discountValue;

        return promoName;
    },

    getInvalidFields: function (form) {
        var invalidFields = [];
        form.getFields().filterBy(function (field) {
            if (field.validate()) return;
            invalidFields.push(field);
        });
        return invalidFields;
    },

    showHistory: function (record, panel) {
        var model = panel.getBaseModel();

        Ext.widget('basereviewwindow', { items: Ext.create('App.view.tpm.promo.HistoricalPromo', { baseModel: model }) })
            .show().down('grid').getStore()
            .setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(record)
            });
    },

    getRecordId: function (record) {
        var idProperty = record.idProperty;
        return record.get(idProperty);
    },

    setButtonState: function (window, visible) {
        window.down('#changePromo').setVisible(!visible);
        window.down('#savePromo').setVisible(visible);
        window.down('#saveAndClosePromo').setVisible(visible);
    },

    setTabsState: function (promoWindow, disabled) {
        promoWindow.down('#history').setDisabled(disabled);
        promoWindow.down('#decline').setDisabled(disabled);

        if (disabled) {
            promoWindow.down('#history').addClass('disabled');
            promoWindow.down('#decline').addClass('disabled');
        } else {
            promoWindow.down('#history').removeCls('disabled');
            promoWindow.down('#decline').removeCls('disabled');
        }

        promoWindow.down('#budget').setDisabled(disabled);
        promoWindow.down('#demand').setDisabled(disabled);
        promoWindow.down('#finance').setDisabled(disabled);
    },

    getColorEndSave: function (window, record, closeWindow, scope) {
        var picker = window.down('[name=ColorId]');
        var filter = window.down('[name=ProductFilter]').getValue();
        var parameters = {
            $actionName: 'GetSuitable',
            $method: 'POST',
            productFilter: filter
        };
        window.setLoading(true);
        breeze.EntityQuery
            .from('Colors')
            .withParameters(parameters)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                window.setLoading(false);
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {
                    if (!result.data.isFilters && result.data.length == 1) {
                        record.set('ColorId', result.data[0]);
                        scope.saveModel(record, window, closeWindow);
                        if (!closeWindow) {
                            scope.setTabsState(window, false);
                            var promoStatusField = window.down('searchfield[name=PromoStatusId]');
                            promoStatusField.setReadOnly(false);
                        }
                    } else {
                        if (!result.data.isFilters) {
                            picker.afterPickerCreate = function (scope) {
                                scope.getStore().setFixedFilter('ColorFilter', {
                                    property: 'Id',
                                    operation: 'In',
                                    value: result.data
                                });
                            }
                        } else {
                            var filterData = result.data;
                            var filterNames = [];
                            var filters = [];
                            if (filterData.brandFilter != null && filterData.brandFilter.length > 0) {
                                filterNames.push('BrandFilter');
                                filters.push({
                                    property: 'BrandId',
                                    operation: 'In',
                                    value: filterData.brandFilter
                                });
                            };
                            if (filterData.techFilter != null && filterData.techFilter.length > 0) {
                                filterNames.push('BrandTechFilter');
                                filters.push({
                                    property: 'BrandTechId',
                                    operation: 'In',
                                    value: filterData.techFilter
                                });
                            };
                            if (filterData.subrangeFilter != null && filterData.subrangeFilter.length > 0) {
                                filterNames.push('SubrangeFilter');
                                filters.push({
                                    property: 'SubrangeId',
                                    operation: 'In',
                                    value: filterData.subrangeFilter
                                });
                            };
                            picker.afterPickerCreate = function (scope) {
                                scope.getStore().setSeveralFixedFilters(filterNames, filters);
                            }
                        }
                        picker.onSelectButtonClick = function (button) {
                            var picker = this.picker,
                                selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
                                selrecord = selModel.hasSelection() ? selModel.getSelection()[0] : null;

                            this.setValue(selrecord);
                            this.afterSetValue(selrecord);
                            picker.close();
                            record.set('ColorId', selrecord.getId());
                            scope.saveModel(record, window, closeWindow);
                            if (!closeWindow) {
                                scope.setTabsState(window, false);
                                var promoStatusField = window.down('searchfield[name=PromoStatusId]');
                                promoStatusField.setReadOnly(false);
                            }
                        };
                        var win = picker.createPicker();

                        if (win) {
                            win.show();
                        }
                        App.Notify.pushInfo(l10n.ns('tpm', 'message').value('ColorChoose'));
                    }
                } else {
                    App.Notify.pushError(result.message);
                }
            })
            .fail(function (data) {
                form.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },

    // =============== Listeners =============== 

    // =============== MECHANIC BEGIN =============== 

    // Событие, срабатывающее при изменении поля MarsMechanicId (TPR, VP, Other, ...).
    marsMechanicListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic');
        var isEdit = true;
        var readonly = false;

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            if (oldValue) {
                mechanicFields.marsMechanicFields.marsMechanicTypeId.reset();
                mechanicFields.marsMechanicFields.marsMechanicDiscount.reset();
            }

            promoController.mechanicTypeChange(
                mechanicFields.marsMechanicFields.marsMechanicId,
                mechanicFields.marsMechanicFields.marsMechanicTypeId,
                mechanicFields.marsMechanicFields.marsMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                readonly,
                isEdit
            );

            promoController.getMechanicTypesByClient(mechanicFields.marsMechanicFields.marsMechanicId,
                mechanicFields.marsMechanicFields.marsMechanicTypeId);

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));

            if (promoController.needUnblockZeroDiscount(promoMechanic, field)) {
                mechanicFields.marsMechanicFields.marsMechanicDiscount.setMinValue(0);
            } else {
                mechanicFields.marsMechanicFields.marsMechanicDiscount.setMinValue(1);
            }
        }
    },

    // Событие, срабатывающее при изменении поля MarsMechanicTypeId (3 + 1, 4 + 1, ...).
    marsMechanicTypeListener: function (field, newValue, oldValue) {
        if (newValue !== oldValue) {
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = field.up('promomechanic');

            if (promoMechanic) {
                var mechanicFields = promoController.getMechanicFields(promoMechanic),
                    promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

                if (mechanicFields.marsMechanicFields.marsMechanicTypeId.rawValue ||
                    mechanicFields.marsMechanicFields.marsMechanicDiscount.rawValue) {

                    if (mechanicFields.marsMechanicFields.marsMechanicTypeId.rawValue) {
                        mechanicFields.marsMechanicFields.marsMechanicDiscount.setValue(
                            mechanicFields.marsMechanicFields.marsMechanicTypeId.valueModels[0].data.Discount
                        );
                    }

                    promoController.setCompletedMechanicStep(true);
                } else {
                    promoController.setNotCompletedMechanicStep(true);
                }

                promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
            }
        }
    },

    // Событие, срабатывающее при изменении поля MarsMechanicDiscount (Скидка).
    marsMechanicDiscountListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            if (mechanicFields.marsMechanicFields.marsMechanicTypeId.rawValue ||
                mechanicFields.marsMechanicFields.marsMechanicDiscount.rawValue) {

                promoController.setCompletedMechanicStep(true);
            } else {
                promoController.setNotCompletedMechanicStep(true);
            }

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
        }
    },

    // Событие, срабатывающее при изменении поля PlanInstoreMechanicId (TPR, VP, Other, ...).
    instoreMechanicListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic');
        var isEdit = true;
        var readonly = false;

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            if (oldValue) {
                mechanicFields.instoreMechanicFields.instoreMechanicTypeId.reset();
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount.reset();
                promoController.setCompletedMechanicStep(false);
            } else {
                promoController.setNotCompletedMechanicStep(false);
            }

            promoController.mechanicTypeChange(
                mechanicFields.instoreMechanicFields.instoreMechanicId,
                mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                readonly,
                isEdit
            );

            promoController.getMechanicTypesByClient(mechanicFields.instoreMechanicFields.instoreMechanicId,
                mechanicFields.instoreMechanicFields.instoreMechanicTypeId);

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));

            if (promoController.needUnblockZeroDiscount(promoMechanic, field)) {
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount.setMinValue(0);
            } else {
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount.setMinValue(1);
            }
        }
    },

    // Событие, срабатывающее при изменении поля PlanInstoreMechanicTypeId (3 + 1, 4 + 1, ...).
    instoreMechanicTypeListener: function (field, newValue, oldValue) {
        if (newValue !== oldValue) {
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = field.up('promomechanic');

            if (promoMechanic) {
                var mechanicFields = promoController.getMechanicFields(promoMechanic),
                    promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

                if (mechanicFields.instoreMechanicFields.instoreMechanicTypeId.rawValue ||
                    mechanicFields.instoreMechanicFields.instoreMechanicDiscount.rawValue) {

                    if (mechanicFields.instoreMechanicFields.instoreMechanicTypeId.rawValue) {
                        mechanicFields.instoreMechanicFields.instoreMechanicDiscount.setValue(
                            mechanicFields.instoreMechanicFields.instoreMechanicTypeId.valueModels[0].data.Discount
                        );
                    }
                    promoController.setCompletedMechanicStep(false);
                } else {
                    promoController.setNotCompletedMechanicStep(false);
                }

                promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
            }
        }
    },

    // Событие, срабатывающее при изменении поля PlanInstoreMechanicDiscount (Скидка).
    instoreMechanicDiscountListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('promomechanic'),
            promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic),
                promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

            if (mechanicFields.instoreMechanicFields.instoreMechanicTypeId.rawValue ||
                mechanicFields.instoreMechanicFields.instoreMechanicDiscount.rawValue) {
                promoController.setCompletedMechanicStep(false);
            }

            promoMechanicButton.setText(promoController.getFullTextForMechanicButton(promoController));
        }
    },

    // Событие, срабатывающее при изменении поля ActualMechanicId (TPR, VP, Other, ...).
    actualMechanicListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('#promoActivity_step1');
        var isEdit = true;
        var readonly = false;

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic);

            if (oldValue) {
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.reset();
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.reset();
            }

            promoController.mechanicTypeChange(
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicId,
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount,
                promoController.getMechanicListForUnlockDiscountField(),
                readonly,
                isEdit
            );

            promoController.getMechanicTypesByClient(mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicId,
                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId);

            if (promoController.needUnblockZeroDiscount(promoMechanic, field)) {
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.setMinValue(0);
            } else {
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.setMinValue(1);
            }
        }
    },

    // Событие, срабатывающее при изменении поля ActualMechanicTypeId (3 + 1, 4 + 1, ...).
    actualMechanicTypeListener: function (field, newValue, oldValue) {
        if (newValue !== oldValue) {
            var promoController = App.app.getController('tpm.promo.Promo'),
                promoMechanic = field.up('#promoActivity_step1');

            if (promoMechanic) {
                var mechanicFields = promoController.getMechanicFields(promoMechanic);

                if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue ||
                    mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.rawValue) {

                    if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue) {
                        mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.setValue(
                            mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.valueModels[0].data.Discount
                        );
                    }
                }
            }
        }
    },

    // Событие, срабатывающее при изменении поля ActualMechanicDiscount (Скидка).
    actualMechanicDiscountListener: function (field, newValue, oldValue) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            promoMechanic = field.up('#promoActivity_step1');

        if (promoMechanic) {
            var mechanicFields = promoController.getMechanicFields(promoMechanic);

            if (mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId.rawValue ||
                mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount.rawValue) {
            }
        }
    },

    // Вызвать при изменении поля MechanicType.
    mechanicTypeChange: function (mechanicId, mechanicTypeId, mechanicDiscount, mechanicListForUnlockDiscountField, readOnly, isEdit) {
        if (readOnly) {
            mechanicId.setDisabled(false);
            mechanicTypeId.setDisabled(false);
            mechanicDiscount.setDisabled(false);
        } else if (mechanicId.rawValue && mechanicListForUnlockDiscountField.some(function (element) { return element !== mechanicId.rawValue; })) {
            mechanicId.setDisabled(false);
            mechanicTypeId.setDisabled(true);
            mechanicDiscount.setDisabled(false);
        } else if (mechanicId.rawValue) {
            mechanicId.setDisabled(false);
            mechanicTypeId.setDisabled(false);
            mechanicDiscount.setDisabled(true);
        } else if (isEdit && !readOnly) {
            mechanicId.setDisabled(false);
            mechanicTypeId.setDisabled(true);
            mechanicDiscount.setDisabled(true);
        } else {
            mechanicId.setDisabled(true);
            mechanicTypeId.setDisabled(true);
            mechanicDiscount.setDisabled(true);
        }
    },

    // Получить список механик, при выборе которых нужно разблокировать поле MechanicTypeId и заблокировать поле MechanicDiscount.
    getMechanicListForUnlockDiscountField: function () {
        return ['VP'];
    },

    // Получить список полей MarsMechanic && получить список полей PlanInstoreMechanic.
    getMechanicFields: function (promoMechanicContainer) {
        return {
            marsMechanicFields: this.getMarsMechanicFields(promoMechanicContainer),
            instoreMechanicFields: this.getPlanInstoreMechanicFields(promoMechanicContainer),
            actualInstoreMechanicFields: this.getActualInstoreMechanicFields(promoMechanicContainer),
        }
    },

    // Получить список полей MarsMechanic.
    getMarsMechanicFields: function (promoMechanic) {
        return {
            marsMechanicId: promoMechanic.down('searchcombobox[name=MarsMechanicId]'),
            marsMechanicTypeId: promoMechanic.down('searchcombobox[name=MarsMechanicTypeId]'),
            marsMechanicDiscount: promoMechanic.down('numberfield[name=MarsMechanicDiscount]')
        }
    },

    // Получить список полей PlanInstoreMechanic.
    getPlanInstoreMechanicFields: function (promoMechanic) {
        return {
            instoreMechanicId: promoMechanic.down('searchcombobox[name=PlanInstoreMechanicId]'),
            instoreMechanicTypeId: promoMechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]'),
            instoreMechanicDiscount: promoMechanic.down('numberfield[name=PlanInstoreMechanicDiscount]')
        }
    },

    // Получить список полей ActualsInstoreMechanic.
    getActualInstoreMechanicFields: function (promoMechanic) {
        return {
            actualInstoreMechanicId: promoMechanic.down('searchcombobox[name=ActualInstoreMechanicId]'),
            actualInstoreMechanicTypeId: promoMechanic.down('searchcombobox[name=ActualInstoreMechanicTypeId]'),
            actualInStoreDiscount: promoMechanic.down('numberfield[name=ActualInStoreDiscount]')
        }
    },

    //фильтруем записи в списке MechanicType по клиенту
    getMechanicTypesByClient: function (mechanicField, mechanicTypeField) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            window = Ext.ComponentQuery.query('promoeditorcustom')[0],
            promoclient = window.down('promoclient'),
            mechanicTypeStore = mechanicTypeField.getStore();

        var fieldValue = mechanicField.rawValue;

        if (fieldValue == promoController.getMechanicListForUnlockDiscountField()) {
            var client = promoclient.clientTreeRecord;

            if (client !== null) {
                mechanicTypeStore.getProxy().extraParams = {
                    byClient: breeze.DataType.Boolean.fmtOData('true'),
                    clientTreeId: breeze.DataType.Int32.fmtOData(client.Id),
                };

                mechanicTypeStore.load();
            }
        }
    },

    setNotCompletedMechanicStep: function (isMars) {
        var promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];

        if (isMars) promoMechanicButton.isMarsMechanicsComplete = false;
        else promoMechanicButton.isInstoreMechanicsComplete = false;
        promoMechanicButton.addCls('notcompleted');
        promoMechanicButton.setGlyph(0xf130);
        promoMechanicButton.isComplete = false;
    },

    setCompletedMechanicStep: function (isMars) {
        var promoMechanicButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step3]')[0];
        if (isMars) promoMechanicButton.isMarsMechanicsComplete = true;
        else promoMechanicButton.isInstoreMechanicsComplete = true;
        if (promoMechanicButton.isInstoreMechanicsComplete && promoMechanicButton.isMarsMechanicsComplete) {
            promoMechanicButton.removeCls('notcompleted');
            promoMechanicButton.setGlyph(0xf133);
            promoMechanicButton.isComplete = true;
        }
    },

    getFullTextForMechanicButton: function (promoController) {
        var promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
            mechanicFields = promoController.getMechanicFields(promoMechanic);

        var marsText = this.getPartTextMarsMechanicButton(
            mechanicFields.marsMechanicFields.marsMechanicId,
            mechanicFields.marsMechanicFields.marsMechanicDiscount
        );

        var instoreText = this.getPartTextMarsMechanicButton(
            mechanicFields.instoreMechanicFields.instoreMechanicId,
            mechanicFields.instoreMechanicFields.instoreMechanicDiscount
        );

        return text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep3') + '</b><br><p>Mars: ' + marsText + '<br>Instore Assumption: ' + instoreText + '</p>';
    },

    getPartTextMarsMechanicButton: function (mechanicId, mechanicDiscount) {
        if (mechanicId.rawValue !== '' && mechanicId.rawValue !== null && mechanicId.rawValue !== undefined &&
            mechanicDiscount.rawValue !== '' && mechanicDiscount.rawValue !== null && mechanicDiscount.rawValue !== undefined) {
            return mechanicId.rawValue + ' ' + mechanicDiscount.rawValue + '%';
        } else {
            return '';
        }
    },

    resetFields: function (fields) {
        fields.forEach(function (field) {
            field.reset();
        });
    },

    disableFields: function (fields) {
        fields.forEach(function (field) {
            field.setDisabled(true);
        });
    },

    needUnblockZeroDiscount: function (promoMechanic, mechanicIdField) {
        var promoController = App.app.getController('tpm.promo.Promo');
        var mechanicIdRawValuesNamesForUnblockZeroDiscountInOut = promoController.getMechanicIdRawValuesNamesForUnblockZeroDiscountInOut();
        var mechanicIdRawValuesNamesForUnblockZeroDiscountRegular = promoController.getMechanicIdRawValuesNamesForUnblockZeroDiscountRegular();
        var mechanicIdRawValuesNamesForUnblockZeroDiscountLoyalty = promoController.getMechanicIdRawValuesNamesForUnblockZeroDiscountLoyalty();
        var window = promoMechanic.up('promoeditorcustom');
        var record = promoController.getRecord(window);

        if ((window.isInOutPromo || (record && record.data.InOut)) && mechanicIdRawValuesNamesForUnblockZeroDiscountInOut.some(function (x) { return x == mechanicIdField.getRawValue() })) {
            return true;
        }
        if ((window.promotype.split(' ')[0] == 'Regular' || (record && record.data.PromoTypesName.split(' ')[0] == 'Regular')) && mechanicIdRawValuesNamesForUnblockZeroDiscountRegular.some(function (x) { return x == mechanicIdField.getRawValue() })) {
            return true;
        }
        if ((window.promotype.split(' ')[0] == 'Loyalty' || (record && record.data.PromoTypesName.split(' ')[0] == 'Loyalty')) && mechanicIdRawValuesNamesForUnblockZeroDiscountLoyalty.some(function (x) { return x == mechanicIdField.getRawValue() })) {
            return true;
        }
        return false;
    },

    getMechanicIdRawValuesNamesForUnblockZeroDiscountInOut: function () {
        return ['Other'];
    },
    getMechanicIdRawValuesNamesForUnblockZeroDiscountRegular: function () {
        return ['Other'];
    },
    getMechanicIdRawValuesNamesForUnblockZeroDiscountLoyalty: function () {
        return ['Coupons', 'Points', 'Programs'];
    },

    // =============== MECHANIC END =============== 

    // хранит столбец плагина разворота строки
    columnExpander: null,
    // true - если изменили набор столбцов через "настроить таблицу"
    columnsUpdated: false,

    onPromoGridStoreLoad: function () {
        var grid = Ext.ComponentQuery.query('promo directorygrid')[0];
        /*     toolbar = grid.up('promo').down('custombigtoolbar');
         if (grid) {
             var selectionModel = grid.getSelectionModel();
             if (selectionModel.hasSelection()) {
                 var record = selectionModel.getSelection()[0];
                 //если производится расчет промо, кнопка Изменить должна быть недоступна для нажатия
                 if (record && record.data && record.data.Calculating) {
                     toolbar.down('#updatebutton').setDisabled(true);
                 } else {
                     toolbar.down('#updatebutton').setDisabled(false);
                 }
             }
         }*/
        // возвращаем плагин разворота строки, только если изменился набор столбцов
        if (this.columnExpander && grid) {
            var column = Ext.create('Ext.grid.column.Column', this.columnExpander);
            grid.headerCt.insert(0, column);
            this.columnExpander = null;
            grid.getView().refresh();
        }
    },

    onGridSettingsClick: function (button) {
        var grid = button.up('promo').down('grid');
        var columns = grid.headerCt.query('gridcolumn');

        // удаляем столбец плагина для разворачивания строки
        grid.headerCt.remove(columns[0]);
        this.columnExpander = columns[0];
        this.columnsUpdated = false;
        grid.getView().refresh();
    },

    // аккуратно, следующие методы для gridsettingswindow вызываются не только в промо, поэтому стоит условие
    onGridSettingsSaveClick: function () {
        var grid = Ext.ComponentQuery.query('promo directorygrid');
        this.columnsUpdated = grid.length > 0;
    },

    onGridSettingsWindowClose: function () {
        var grid = Ext.ComponentQuery.query('promo directorygrid')[0];

        // если набор полей не изменен возвращаем плагин здесь
        // иначе он вернется в методе onPromoGridStoreLoad
        if (grid && this.columnExpander && !this.columnsUpdated) {
            var column = Ext.create('Ext.grid.column.Column', this.columnExpander);
            grid.headerCt.insert(0, column);
            grid.getView().refresh();
            this.columnExpander = null;
        }
    },

    onPromoBudgetsAfterRender: function (panel) {
        var fieldsForSum = ['PlanPromoTIShopper', 'PlanPromoTIMarketing', 'PlanPromoBranding', 'PlanPromoBTL', 'PlanPromoCostProduction'];

        this.calculateShopperTI();
        this.calculateTotalCost('#promoBudgets_step1', 'PlanPromoCost', fieldsForSum);

        fieldsForSum = ['ActualPromoTIShopper', 'ActualPromoTIMarketing', 'ActualPromoBranding', 'ActualPromoBTL', 'ActualPromoCostProduction'];
        this.calculateTotalCost('#promoBudgets_step1', 'ActualPromoCost', fieldsForSum);

        /*var needRecountUplift = Ext.ComponentQuery.query('[name=NeedRecountUplift]')[0];
        var model = this.getRecord(panel.up('window'));
        var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];
    
        if (model) {
            if (model.data.NeedRecountUplift === true) {
                needRecountUplift.setValue(false);
            } else {
                needRecountUplift.setValue(true);
            }
        }
    
        if (needRecountUplift.disabled === true) {
            planUplift.setReadOnly(true);
            planUplift.addCls('readOnlyField');
        }
    
        if (Ext.ComponentQuery.query('#changePromo')[0].isVisible() === false) {
            needRecountUplift.setDisabled(false);
    
            if (needRecountUplift.value === true) {
                planUplift.setReadOnly(false);
                planUplift.removeCls('readOnlyField');
            }
        }*/


        // Установка стиля для readOnly полей.
        var fieldsForReadOnlyCls = [
            'ActualPromoTIShopper', 'PlanPromoTIShopper', 'PlanPromoTIMarketing', 'ActualPromoTIMarketing', 'PlanPromoCostProduction', 'ActualPromoCostProduction', 'PlanPromoCost', 'ActualPromoCost'
        ];

        this.setReadOnlyFields(fieldsForReadOnlyCls, false);

        // Блокировка кнопки Add Promo Support в режиме просмотра.
        //var addSubItemButtons = panel.query('#addSubItem');
        //addSubItemButtons.forEach(function (button) {
        //    button.setDisabled(panel.up('window').down('#changePromo').isVisible());
        //})
    },

    setReadOnlyFields: function (fieldNames, all) {
        fieldNames.forEach(function (fieldName) {
            var fields = Ext.ComponentQuery.query('[name=' + fieldName + ']');
            for (var i = 0; i < fields.length; i++) {
                //fields[i].addCls('readOnlyField');

                if (!all)
                    break;
            }
        });
    },

    setFieldsReadOnlyForSomeRole: function (promoeditorcustom) {
        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
        var fieldsWithCrud = Ext.ComponentQuery.query('[crudAccess*=' + currentRole + ']');

        // ------------------------ Client and Product -----------------------          
        var promoClientForm = promoeditorcustom.down('promoclient');
        var promoProductForm = promoeditorcustom.down('promobasicproducts');

        var clientCrudAccess = ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];
        var productCrudAccess = ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'];

        if (clientCrudAccess.indexOf(currentRole) === -1) {
            promoClientForm.down('#choosePromoClientBtn').setDisabled(true);
        }

        if (productCrudAccess.indexOf(currentRole) === -1) {
            promoProductForm.setDisabledBtns(true);
        }

        // ------------------------ Adjustment-----------------------    
        var adjustmentSlider = promoeditorcustom.down('sliderfield[name=DeviationCoefficient]');
        var adjustmentNumber = promoeditorcustom.down('numberfield[name=Adjustment]');

        if (adjustmentSlider.crudAccess.indexOf(currentRole) === -1) {
            adjustmentSlider.setDisabled(true);
            adjustmentNumber.setReadOnly(true);
        }
        else {
            adjustmentSlider.setReadOnly(false);
            adjustmentNumber.setReadOnly(false);
            adjustmentSlider.setDisabled(false);
            adjustmentNumber.setDisabled(false);
        }

        // ---------------Other---------------  
        fieldsWithCrud.forEach(function (field) {
            field.removeCls('readOnlyField');
            if (field.disabled) {
                field.setDisabled(false);
            }
            if (field.readOnly) {
                field.setReadOnly(false);
            }
        });
        var isInOut = promoeditorcustom.isInOutPromo;
        if (isInOut) {
            promoeditorcustom.down('checkbox[name=NeedRecountUplift]').setDisabled(true);
        }
        // Блокировка кнопок Add Promo Support для роли DemandPlanning. 
        // Блокировка редактирования Growth Acceleration
        // Блокировка редактирования Calendar priority
        if (currentRole == 'DemandPlanning') {
            //var addSubItemButtons = Ext.ComponentQuery.query('#addSubItem');
            //if (addSubItemButtons.length > 0) {
            //    addSubItemButtons.forEach(function (button) {
            //        button.disabled = true;
            //        button.setDisabled = function () { return true; }
            //    });
            //}

            var growthAccelerationCheckbox = promoeditorcustom.down('[name=GrowthAccelerationCheckbox]');
            growthAccelerationCheckbox.setReadOnly(true);

            var calendarPrioritySlider = promoeditorcustom.down('[name=priority]');
            calendarPrioritySlider.setReadOnly(true);

            var client = promoeditorcustom.down('container[name=promo_step1]');
            client.down('[id=OffInvoice]').setDisabled(true);
            client.down('[id=OnInvoice]').setDisabled(true);
        }

        // ------------------------ Mechanic->ApolloExport -----------------------    
        var apolloExportCheckbox = promoeditorcustom.down('[name=ApolloExportCheckbox]');

        if (apolloExportCheckbox.crudAccess.indexOf(currentRole) === -1) {
            apolloExportCheckbox.setDisabled(true);
        }
        else {
            apolloExportCheckbox.setReadOnly(false);
            apolloExportCheckbox.setDisabled(false);
        }
    },

    onPromoActivityAfterRender: function (panel) {
        var me = this;
        // Костыль для срабатывания события из PromoEditorCustomScroll.js
        var formStep2 = panel.down('#promoActivity_step2');
        formStep2.setHeight(600);

        var needRecountUplift = Ext.ComponentQuery.query('[itemId=PromoUpliftLockedUpdateCheckbox]')[0];
        var model = this.getRecord(panel.up('window'));
        //var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

        if (model) {
            if (model.data.NeedRecountUplift === true) {
                needRecountUplift.setValue(false);
            } else {
                needRecountUplift.setValue(true);
            }
        }

        //      if (needRecountUplift.disabled === true || Ext.ComponentQuery.query('#changePromo')[0].isVisible()) {
        //          planUplift.changeEditable(false);
        //          planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33f);
        //          planUplift.up('container').setReadable(true);
        //}

        //if (Ext.ComponentQuery.query('#changePromo')[0].isVisible() === false) {
        //	if (model.data.InOut) {
        //		needRecountUplift.setDisabled(true);
        //	} else {
        //		needRecountUplift.setDisabled(false);
        //	}

        //          if (needRecountUplift.value === true) {
        //              planUplift.changeEditable(true);
        //              planUplift.up('container').down('button[itemId=GlyphLock]').setGlyph(0xf33e);
        //              planUplift.up('container').setReadable(true);
        //	}
        //}

        //var promoStatusName = model.get('PromoStatusName')
        ////Начавшиеся promo не редактируются uplift
        //var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoStatusName) >= 0);
        //if (isPromoWasStarted) {
        //	me.blockStartedPromoUplift();
        //}

        //// Установка стиля для readOnly полей.
        //var fieldsForReadOnlyCls = [
        //	'PlanPromoBaselineLSV', 'PlanPromoIncrementalLSV', 'PlanPromoLSV', 'PlanPromoPostPromoEffectLSV',
        //	'ActualPromoUpliftPercent', 'ActualPromoBaselineLSV', 'ActualPromoIncrementalLSV', 'ActualPromoLSV', 'ActualPromoLSVByCompensation', 'ActualPromoPostPromoEffectLSV',
        //];

        //me.setReadOnlyFields(fieldsForReadOnlyCls, false);
    },


    setReadOnlyForChildrens: function (window, status, isPromoEditable, IsInOut) {
        if (window) {
            var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
            this.setReadOnlyForChildrensRecursion(this, window, currentRole, status, false, isPromoEditable, IsInOut);
        } else {
            console.warn('Window does not exist');
        }
    },

    setReadOnlyForChildrensRecursion: function (me, window, currentRole, currentStatus, isParentEditable, isPromoEditable, IsInOut) {
        window.items.items.forEach(function (item) {
            var isThisEditable = me.isActionAllowed(item, currentRole, currentStatus, isParentEditable, IsInOut, isPromoEditable);
            if (item.setEditable && (item.xtype == 'triggerfielddetails' || item.xtype == 'triggerfield')) {
                item.setEditable(isThisEditable);
            } else if (item.setDisabled && (item.xtype == 'checkbox' || item.xtype == 'button')) {
                item.setDisabled(!isThisEditable);
            } else if (item.setReadOnly && item.xtype != 'triggerfielddetails') {
                item.setReadOnly(!isThisEditable);
            };
            if (item.items && item.items.items) {
                me.setReadOnlyForChildrensRecursion(me, item, currentRole, currentStatus, isThisEditable, isPromoEditable, IsInOut);
            }
        })
    },

    isActionAllowed: function (item, currentRole, currentStatus, isParentEditable, IsInOut, isPromoEditable) {
        var isAvailable = isParentEditable;
        if (isPromoEditable || item.availableInReadOnlyPromo) {
            if (item.noneCanEdit) {
                isAvailable = false;
            } else {
                //Если промо InOut и есть отдельный конфиг ролей для инаута
                if (IsInOut && item.availableRoleStatusActionsInOut) {
                    if (item.availableRoleStatusActionsInOut[currentRole]) {
                        if (item.availableRoleStatusActionsInOut[currentRole].includes(currentStatus)) {
                            isAvailable = true;
                        } else {
                            isAvailable = false;
                        }
                    }
                } else {
                    if (item.availableRoleStatusActions && item.availableRoleStatusActions[currentRole]) {
                        if (item.availableRoleStatusActions[currentRole].includes(currentStatus)) {
                            isAvailable = true;
                        } else {
                            isAvailable = false;
                        }
                    }
                }
            }
        }
        return isAvailable;
    },

    // При изменении полей в Budgets Step 1 перерасчитываем Total Cost
    // А также меняем значения в Activity
    onPromoBudgetsStep1Change: function (field, newValue) {
        if (field.getName() != 'PlanPromoCost') {
            var fieldsForSum = ['PlanPromoTIShopper', 'PlanPromoTIMarketing', 'PlanPromoBranding', 'PlanPromoBTL', 'PlanPromoCostProduction'];
            this.calculateTotalCost('#promoBudgets_step1', 'PlanPromoCost', fieldsForSum);

            fieldsForSum = ['ActualPromoTIShopper', 'ActualPromoTIMarketing', 'ActualPromoBranding', 'ActualPromoBTL', 'ActualPromoCostProduction'];
            this.calculateTotalCost('#promoBudgets_step1', 'ActualPromoCost', fieldsForSum);
        }

        this.onCalculationPlanChange(field, newValue);
    },

    // Расчет Shopper TI в Promo Budgets Step 1
    calculateShopperTI: function () {
        /*var window = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var promomechanic = window.down('promomechanic');
        var marsMechanicDiscount = promomechanic.down('numberfield[name=MarsMechanicDiscount]').getValue();
        var totalPromoLSV = window.down('#calculations_step2').down('numberfield[name=PlanPromoIncrementalLSV]').getValue();
        var shopperTIField = window.down('#calculations_step1').down('numberfield[name=PlanPromoTIShopper]');
    
        // если нет значения в одном из полей, то сбрасываем
        if (totalPromoLSV != null && marsMechanicDiscount != null)
            shopperTIField.setValue(totalPromoLSV * marsMechanicDiscount / 100.0);
        else
            shopperTIField.setValue(null);*/
    },

    // Расчет суммы:
    // panelStep  - id панели шага
    // totalField - name поля, куда пишется результат
    // sumFields  - name полей, значения которых суммируются
    calculateTotalCost: function (panelStep, totalField, sumFields, isPlan) {
        var panel = Ext.ComponentQuery.query('promoeditorcustom')[0].down(panelStep);
        var totalCostField = panel.down('numberfield[name=' + totalField + ']');
        var total = 0;

        for (var i = 0; i < sumFields.length; i++) {
            var val = panel.down('[name=' + sumFields[i] + ']').getValue();
            if (val !== null && val != undefined && val != '')
                total += parseFloat(val);
        }

        totalCostField.setValue(total);
    },

    onMarsMechanicDiscountChange: function (field) {
        this.calculateShopperTI();
    },

    onPlanPromoIncrementalLSVChange: function (field) {
        this.calculateShopperTI();
    },

    hideEditButtonForSomeRole: function () {
        var resource = 'Promoes';
        var allowedActions = [];

        // Получаем список точек достуа текущей роли и ресурса.
        var accessPointsForCurrentRoleAndResouce = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
            return point.Resource === resource;
        });

        // Сбор всех actions для текущей роли и ресурса.
        accessPointsForCurrentRoleAndResouce.forEach(function (point) {
            if (!allowedActions.some(function (action) { return action === point.Action })) {
                allowedActions.push(point.Action);
            }
        });

        if (!allowedActions.some(function (action) { return action === 'Patch'; })) {
            Ext.ComponentQuery.query('#changePromo')[0].hide();
        }
    },

    // при изменении полей в Calculations меняем и в Promo Activity
    onCalculationPlanChange: function (field, newValue) {
        var panel = field.up('promoeditorcustom');
        var fieldActivity = panel.down('promoactivity').down('numberfield[name =' + field.getName() + ']');

        if (fieldActivity) {
            fieldActivity.setValue(newValue);
        }
    },

    getRecord: function (window) {
        var record = null;

        if (window.isCreating) {
            record = Ext.create('App.model.tpm.promo.Promo');
        } else if (window.model != null) {
            record = window.model;
        } else if (window.assignedRecord != null) {
            record = window.assignedRecord;
        } else {
            // Если запись обозначена в гриде - брать её
            var promoGrid = Ext.ComponentQuery.query('#promoGrid')[0];
            record = window.assignedRecord ? window.assignedRecord : promoGrid.down('directorygrid').getSelectionModel().getSelection()[0];
        }

        return record;
    },

    setReadOnlyProperty: function (readOnlyProperty, elements) {
        elements.forEach(function (el) {
            el.setReadOnly(readOnlyProperty);
            if (readOnlyProperty) {
                el.addClass('readOnlyField');
            } else {
                el.removeCls('readOnlyField');
            }
        });
    },

    onDeleteButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('deleteWindowTitle'),
                msg: l10n.ns('core').value('deleteConfirmMessage'),
                fn: onMsgBoxClose,
                scope: this,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('core', 'buttons').value('delete'),
                    no: l10n.ns('core', 'buttons').value('cancel')
                }
            });
        } else {
            console.log('No selection');
        }

        function onMsgBoxClose(buttonId) {

            if (buttonId === 'yes') {
                var record = selModel.getSelection()[0],
                    store = grid.getStore(),
                    view = grid.getView(),
                    currentIndex = store.indexOf(record),
                    pageIndex = store.getPageFromRecordIndex(currentIndex),
                    endIndex = store.getTotalCount() - 2; // 2, т.к. после удаления станет на одну запись меньше

                currentIndex = Math.min(Math.max(currentIndex, 0), endIndex);
                panel.setLoading(l10n.ns('core').value('deletingText'));

                record.destroy({
                    scope: this,
                    success: function () {
                        selModel.deselectAll();
                        selModel.clearSelections();
                        store.data.removeAtKey(pageIndex);
                        store.totalCount--;

                        if (store.getTotalCount() > 0) {
                            view.bufferedRenderer.scrollTo(currentIndex, true, function () {
                                view.refresh();
                                panel.setLoading(false);
                                grid.setLoadMaskDisabled(true);
                                view.focusRow(currentIndex);
                            });
                        } else {
                            grid.setLoadMaskDisabled(true);
                            store.on({
                                single: true,
                                load: function (records, operation, success) {
                                    panel.setLoading(false);
                                    grid.setLoadMaskDisabled(false);
                                }
                            });
                            store.load();
                        }
                    },
                    failure: function () {
                        panel.setLoading(false);
                    }
                });
            }
        }
    },

    // =============== FILL PROMO EVENTS ===============
    // здесь описаны события загрузок различных частей карточки PROMO

    // количество загрузившихся элементов, всего 2
    countLoadedComponents: 0,

    // перезаполнить промо
    reFillPromoForm: function (window, record, grid) {
        this.countLoadedComponents = 0;
        var readOnly = window.readOnly ? window.readOnly : false;
        this.fillPromoForm(window, record, readOnly, false, grid);
    },

    // метод вызывать СТРОГО ОДИН РАЗ ЗА ОДНО ОТКРЫТИЕ PROMO
    // привязать события загрузок
    bindAllLoadEvents: function (window, record, isCopy) {
        this.bindEventMarsMechanicLoad(window, record);
        this.bindEventPlanInstoreMechanicLoad(window, record);
        this.bindEventActualInstoreMechanicLoad(window, record);
    },

    // если все компоненты загрузились, снимаем маску загрузки
    checkLoadingComponents: function () {
        if (this.countLoadedComponents < 0) {
            this.countLoadedComponents++;
        }

        if (this.countLoadedComponents == 0) {
            var window = Ext.ComponentQuery.query('promoeditorcustom');

            if (window.length > 0) {
                this.hideEditButtonForSomeRole();
                if (window[0].GetIfAllProductsInSubrange === false) {
                    window[0].setLoading(false);
                }

                this.initSignalR(window[0]);
            }
        }
    },

    bindEventMarsMechanicLoad: function (window, record) {
        var me = this;
        var mechanic = window.down('container[name=promo_step3]');

        var marsMechanicId = mechanic.down('searchcombobox[name=MarsMechanicId]');
        var marsMechanicTypeId = mechanic.down('searchcombobox[name=MarsMechanicTypeId]');
        var marsMechanicDiscount = mechanic.down('numberfield[name=MarsMechanicDiscount]');

        marsMechanicId.addListener('change', me.marsMechanicListener);
        marsMechanicTypeId.addListener('change', me.marsMechanicTypeListener);
        marsMechanicDiscount.addListener('change', me.marsMechanicDiscountListener);
    },

    bindEventPlanInstoreMechanicLoad: function (window, record) {
        var me = this;
        var mechanic = window.down('container[name=promo_step3]');

        var instoreMechanicId = mechanic.down('searchcombobox[name=PlanInstoreMechanicId]');
        var instoreMechanicTypeId = mechanic.down('searchcombobox[name=PlanInstoreMechanicTypeId]');
        var instoreMechanicDiscount = mechanic.down('numberfield[name=PlanInstoreMechanicDiscount]');

        instoreMechanicId.addListener('change', me.instoreMechanicListener);
        instoreMechanicTypeId.addListener('change', me.instoreMechanicTypeListener);
        instoreMechanicDiscount.addListener('change', me.instoreMechanicDiscountListener);
    },

    bindEventActualInstoreMechanicLoad: function (window, record) {
        var me = this;
        var mechanic = window.down('container[name=promoActivity_step1]');

        var actualInstoreMechanicId = mechanic.down('searchcombobox[name=ActualInstoreMechanicId]');
        var actualInstoreMechanicTypeId = mechanic.down('searchcombobox[name=ActualInstoreMechanicTypeId]');
        var actualInStoreDiscount = mechanic.down('numberfield[name=ActualInStoreDiscount]');

        actualInstoreMechanicId.addListener('change', me.actualMechanicListener);
        actualInstoreMechanicTypeId.addListener('change', me.actualMechanicTypeListener);
        actualInStoreDiscount.addListener('change', me.actualMechanicDiscountListener);
    },
    // =============== END FILL PROMO EVENTS ===============

    changeStatusPromo: function (promoId, statusId, window) {
        var params = 'id=' + promoId + '&promoNewStatusId=' + statusId;
        var grid = Ext.ComponentQuery.query('#promoGrid')[0];
        var me = this;

        window.setLoading(l10n.ns('core').value('savingText'));

        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/ChangeStatus?' + params,
            type: 'POST',
            success: function (data) {
                // привет EXT!
                App.model.tpm.promo.Promo.load(promoId, {
                    callback: function (newModel, operation) {
                        var directorygrid = grid ? grid.down('directorygrid') : null;

                        window.promoId = data.Id;
                        window.model = newModel;
                        me.reFillPromoForm(window, newModel, directorygrid);
                    }
                });

                // Если создание из календаря - обновляем календарь
                var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                if (scheduler) {
                    scheduler.resourceStore.reload();
                    scheduler.eventStore.reload();
                }
                me.updateStatusHistoryState();
            },
            error: function (data) {
                // при неудаче возвращаем старый статус
                if (window.previousStatusId) {
                    window.statusId = window.previousStatusId;
                }

                window.setLoading(false);
                App.Notify.pushError(data.statusText);
            }
        });
    },

    onPromoGridSelectionChange: function (selModel, selected) {
        this.onGridSelectionChange(selModel, selected);
        selModel.view.up('#viewcontainer').down('#canchangeresponsible').setDisabled(false);
    },

    onShowLogButtonClick: function (button) {
        //if (button.promoId) {
        this.onPrintPromoLog(button.up('promoeditorcustom'));
        //}
    },

    // открывает грид с promoproducts для промо
    onActivityUploadPromoProductsClick: function (button) {
        var currentRole = App.UserInfo.getCurrentRole();
        var promoForm = button.up('promoeditorcustom');
        var promoProductWidget = Ext.widget('promoproduct');
        var record = this.getRecord(promoForm);
        var me = this;

        if (promoProductWidget) {
            promoProductWidget.record = record;
            promoProductWidget.addListener('afterrender', function () {
                var toolbar = promoProductWidget.down('custombigtoolbar');
                var importBtn = promoProductWidget.down('[action=FullImportXLSX]');
                var importPluBtn = promoProductWidget.down('[action=FullImportPluXLSX]');

                toolbar.down('#createbutton').hide();
                importBtn.action += '?promoId=' + record.get('Id');
                importPluBtn.action += '?promoId=' + record.get('Id');

                var tmplTLC = promoProductWidget.down('#loadimporttemplatexlsxbuttonTLC');
                var tmplNotTLC = promoProductWidget.down('#loadimporttemplatexlsxbutton');
                if ((currentRole.SystemName.toLowerCase() == 'keyaccountmanager' || currentRole.SystemName.toLowerCase() == 'supportadministrator') && record.data.LoadFromTLC) {
                    tmplTLC.show();
                    tmplNotTLC.hide();
                } else if (currentRole.SystemName.toLowerCase() != 'keyaccountmanager' && currentRole.SystemName.toLowerCase() != 'supportadministrator' && record.data.LoadFromTLC) {
                    importBtn.hide();
                } else {
                    tmplNotTLC.show();
                    tmplTLC.hide();
                }
            })

            // загружать нужно только для текущего промо
            var store = promoProductWidget.down('grid').getStore();
            store.getProxy().extraParams.updateActualsMode = true;
            store.getProxy().extraParams.promoIdInUpdateActualsMode = record.get('Id');

            store.load();

            //store.setFixedFilter('PromoIdFilter', {
            //	property: 'PromoId',
            //	operation: 'Equals',
            //	value: record.get('Id')
            //})

            Ext.widget('selectorwindow', {
                title: 'Upload Actuals',
                itemId: Ext.String.format('{0}_{1}_{2}', 'activityUploadPromoProducts', promoProductWidget.getXType(), 'selectorwindow'),
                items: [promoProductWidget],
                buttons: [{
                    text: 'Close',
                    style: { "background-color": "#3F6895" },
                    listeners: {
                        click: function (button) {
                            button.up('selectorwindow').close();
                        }
                    }
                }],
                listeners: {
                    beforeclose: function () {
                        // проверяем идут ли расчеты
                        App.model.tpm.promo.Promo.load(record.get('Id'), {
                            callback: function (newModel, operation) {
                                if (newModel.data.Calculating) {
                                    var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                                    var directorygrid = grid ? grid.down('directorygrid') : null;

                                    promoForm.model = newModel;
                                    me.reFillPromoForm(promoForm, newModel, directorygrid);

                                    // Если создание из календаря - обновляем календарь
                                    var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                                    if (scheduler) {
                                        scheduler.resourceStore.reload();
                                        scheduler.eventStore.reload();
                                    }
                                }
                            }
                        });
                    }
                }
            }).show();
        }
        if (currentRole.SystemName.toLowerCase() == 'supportadministrator' && promoForm.promoStatusSystemName != "Finished" && promoForm.promoStatusSystemName != "Closed") {
            Ext.MessageBox.alert(l10n.ns('tpm', 'PromoActivity').value('Warning'), l10n.ns('tpm', 'PromoActivity').value('uploadActualsStatus'));
        };
    },

    //окно логов
    onPrintPromoLog: function (window, grid, close) {
        //var printPromoCalculatingWin = Ext.create('App.view.tpm.promo.PromoCalculatingWindow');
        //printPromoCalculatingWin.show();

        var calculatingInfoWindow = Ext.create('App.view.tpm.promocalculating.CalculatingInfoWindow');
        calculatingInfoWindow.on({
            beforeclose: function () {
                if ($.connection.logHub)
                    requestHub($.connection.logHub.server.unsubscribeLog);
            }
        });

        calculatingInfoWindow.show();
        requestHub($.connection.logHub.server.subscribeLog);
    },

    createTaskCheckCalculation: function (window) {
        //var record = this.getRecord(window);
        //var me = this;

        //record.set('Calculating', true);

        //var taskForLog = {
        //    run: function () {
        //        // Периодическое чтение лога в окно и проверка на окончание
        //        var params = 'promoId=' + record.get('Id');
        //        Ext.Ajax.request({
        //            method: 'POST',
        //            url: 'odata/Promoes/ReadPromoCalculatingLog?' + params,
        //            success: function (response, opts) {
        //                var result = Ext.JSON.decode(response.responseText);

        //                // если промо закрыли, нужно прекратить запросы
        //                if (!window.isVisible()) {
        //                    Ext.TaskManager.stop(taskForLog);
        //                } else {
        //                    var logWindow = Ext.ComponentQuery.query('promocalculatingwindow');

        //                    me.setCalculatingInformation(result);

        //                    //Проверка на закрытие
        //                    if (result.code == -1) { // ошибка во время проверки
        //                        Ext.TaskManager.stop(taskForLog);
        //                        Ext.Msg.alert("Внимание", result.opData);
        //                        me.unBlockPromo(window);
        //                    } else if (result.code == 1) { // задача завершилась
        //                        Ext.TaskManager.stop(taskForLog);
        //                        logWindow.jobEnded = true;
        //                        me.unBlockPromo(window);
        //                    }
        //                }
        //            },
        //            failure: function () {
        //                Ext.Msg.alert('ReadPromoCalculatingLog error');
        //            }
        //        });
        //    },
        //    interval: (1000) * 0.5 // 0.5 секунды
        //};

        //Ext.TaskManager.start(taskForLog);
    },

    setCalculatingInformation: function (result) {
        var infoWindow = Ext.ComponentQuery.query('calculatinginfowindow');

        if (infoWindow.length > 0) {
            Ext.suspendLayouts();

            infoWindow = infoWindow[0];
            var infoGrid = infoWindow.down('grid'),
                gridInfoToolbar = infoGrid.down('gridinfotoolbar'),
                infoStore = infoGrid.getStore(),
                descriptionField = infoWindow.down('triggerfield[name=Task]'),
                statusField = infoWindow.down('triggerfield[name=Status]');

            if (result.status === 'INPROGRESS') {
                this.changeCls(statusField, 'readOnlyField', 'inprogressField');
                this.changeCls(statusField.triggerEl, '', 'inprogressField-trigger');
                this.changeCls(statusField.triggerCell, '', 'inprogressField-trigger-cell');
            } else if (result.status === 'COMPLETE') {
                this.changeCls(statusField, 'inprogressField', 'completeField');
                this.changeCls(statusField.triggerEl, 'inprogressField-trigger', 'completeField-trigger');
                this.changeCls(statusField.triggerCell, 'inprogressField-trigger-cell', 'completeField-trigger-cell');
            } else if (result.status === 'PARTIAL COMPLETE') {
                this.changeCls(statusField, 'inprogressField', 'completeField');
                this.changeCls(statusField.triggerEl, 'inprogressField-trigger', 'completeField-trigger');
                this.changeCls(statusField.triggerCell, 'inprogressField-trigger-cell', 'completeField-trigger-cell');
            } else if (result.status === 'ERROR') {
                if (statusField.hasCls('readOnlyField')) {
                    this.changeCls(statusField, 'readOnlyField', 'errorField');
                    this.changeCls(statusField.triggerEl, '', 'errorField-trigger');
                    this.changeCls(statusField.triggerCell, '', 'errorField-trigger-cell');
                }
                else {
                    this.changeCls(statusField, 'inprogressField', 'errorField');
                    this.changeCls(statusField.triggerEl, 'inprogressField-trigger', 'errorField-trigger');
                    this.changeCls(statusField.triggerCell, 'inprogressField-trigger-cell', 'errorField-trigger-cell');
                }
            }

            descriptionField.setValue(result.description);
            statusField.setValue(result.status);
            // TODO: добавить ERROR

            var logData = [];
            var logText = result.data;
            if (logText) {
                logData = this.parseLogData(logText, infoWindow);
                infoWindow.logText = logText;
            }

            if (logData.length > 0) {
                infoStore.loadData(logData.slice(0, 10000));
                for (var i = 10001; i < logData.length; i += 10000) {
                    infoStore.loadData(logData.slice(i, i + 10000), true);
                }
                this.colorRaws(infoGrid);

                var displayItem = gridInfoToolbar.child('#displayItem'),
                    msg = Ext.String.format(l10n.ns('core', 'gridInfoToolbar').value('gridInfoMsg'), infoStore.data.length);

                if (displayItem) {
                    displayItem.setText(msg);
                }
            }

            // если пользователь вызвал окно лога со всеми сообщениями, то обновляем и его
            var oldLogWindow = Ext.ComponentQuery.query('promocalculatingwindow');
            if (oldLogWindow.length > 0) {
                var textareaOldLogWindow = oldLogWindow[0].down('textarea');

                if (textareaOldLogWindow) {
                    textareaOldLogWindow.setValue(infoWindow.logText);
                }
            }

            Ext.resumeLayouts(true);
        }
    },

    changeCls: function (el, prevCls, nextCls) {
        el.removeCls(prevCls);
        el.addCls(nextCls);
    },

    parseLogData: function (text, window) {
        var data = [],
            startTag = '[',
            endTag = ']:',
            message = '',
            errorType = 'ERROR',
            warningType = 'WARNING',
            infoType = 'INFO',
            timingType = 'TIMING',
            errorLabel = window.down('button[name=logError]'),
            warningLabel = window.down('button[name=logWarning]'),
            messageLabel = window.down('button[name=logMessage]'),
            errorCount = 0,
            warningCount = 0,
            messageCount = 0;

        // ищем по два тега и текст между ними и будет сообщением
        var findedTagFirst = this.findTagMessage(0, text, startTag, endTag, [errorType, warningType, infoType, timingType]);

        do {
            var findedTagSecond = this.findTagMessage(findedTagFirst.indexEnd, text, startTag, endTag, [errorType, warningType, infoType, timingType]);

            if (findedTagFirst.tag != null) {
                if (findedTagFirst.tag === errorType) {
                    errorCount++;
                } else if (findedTagFirst.tag === warningType) {
                    warningCount++;
                } else if (findedTagFirst.tag === infoType) {
                    messageCount++;
                }

                message = findedTagSecond.tag != null ? text.substring(findedTagFirst.indexEnd + endTag.length, findedTagSecond.indexStart) :
                    text.substring(findedTagFirst.indexEnd + endTag.length);

                data.push(Ext.create('App.model.tpm.promocalculating.CalculatingInfoLog', {
                    Type: findedTagFirst.tag,
                    Message: message
                }));

                findedTagFirst = findedTagSecond;
            }

        } while (findedTagFirst.tag != null);

        errorLabel.setText(errorCount + ' Errors');
        warningLabel.setText(warningCount + ' Warnings');
        messageLabel.setText(messageCount + ' Infos');

        return data;
    },

    // ищет тег в сообщении
    findTagMessage: function (startIndex, text, startTag, endTag, typesTag) {
        var indexStart = startIndex;
        var indexEnd = 0;
        var tag = null;

        do {
            indexStart = text.indexOf(startTag, indexStart);
            indexEnd = text.indexOf(endTag, indexStart);

            if (indexStart !== -1) {
                var indexTag = typesTag.indexOf(text.substring(indexStart++ + startTag.length, indexEnd));
                tag = indexTag !== -1 ? typesTag[indexTag] : null;
            }

        } while (tag == null && indexStart !== -1);

        return { indexStart: indexStart - 1, indexEnd: indexEnd, tag: tag };
    },

    blockPromo: function (window, record) {
        var rec = record || this.getRecord(window);
        var toolbar = window.down('customtoptoolbar');

        rec.set('Calculating', true);

        toolbar.addCls('custom-top-panel-calculating');

        var label = toolbar.down('label[name=promoName]');
        if (label) {
            label.setText(label.text + ' — Promo is blocked for recalculations');
        }

        window.down('#changePromo').setDisabled(true);
        toolbar.down('#btn_showlog').addCls('showlog');
        //toolbar.down('#btn_showlog').show();
        toolbar.down('#btn_showlog').promoId = rec.data.Id;
    },

    unBlockPromo: function (window) {
        var record = this.getRecord(window);
        var toolbar = window.down('customtoptoolbar');
        var toolbarbutton = window.down('button[itemId=btn_sendForApproval]').up();
        var me = this;

        if (toolbar && toolbarbutton) {
            toolbar.items.items.forEach(function (item, i, arr) {
                //item.el.setStyle('backgroundColor', '#3f6895');
                if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish',
                    'btn_sendForApproval', 'btn_reject', 'btn_backToDraftPublished', 'btn_approve', 'btn_cancel', 'btn_plan', 'btn_close', 'btn_backToFinished'].indexOf(item.itemId) > -1) {
                    item.setDisabled(false);
                }
            });
            toolbarbutton.items.items.forEach(function (item, i, arr) {
                if (item.xtype == 'button' && ['btn_publish', 'btn_undoPublish',
                    'btn_sendForApproval', 'btn_reject', 'btn_backToDraftPublished', 'btn_approve', 'btn_cancel', 'btn_plan', 'btn_close', 'btn_backToFinished'].indexOf(item.itemId) > -1) {
                    item.setDisabled(false);
                }
            });
            toolbar.removeCls('custom-top-panel-calculating');

            var label = toolbar.down('label[name=promoName]');
            if (label && label.text.lastIndexOf(' — ') !== -1) {
                var text = label.text.substr(0, label.text.lastIndexOf(' — '));
                label.setText(text);
            }

            window.down('#savePromo').setDisabled(false);
            window.down('#saveAndClosePromo').setDisabled(false);
            window.down('#changePromo').setDisabled(false);

            if (this.detailButton) {
                window.down('#closePromo').setVisible(true);
                window.down('#cancelPromo').setVisible(false);
            }
            else {
                window.down('#cancelPromo').setVisible(true);
                window.down('#closePromo').setVisible(false);
            }

            //toolbar.down('#btn_showlog').hide();

            if (record.data.PromoStatusSystemName == 'Draft' && App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'supportadministrator') {
                window.down('#btn_recalculatePromo').hide();
                window.down('#btn_resetPromo').show();
            } else if ((App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'administrator' || App.UserInfo.getCurrentRole().SystemName.toLowerCase() == 'supportadministrator')
                && record.data.PromoStatusSystemName != 'Draft' && record.data.PromoStatusSystemName != 'Cancelled') {
                window.down('#btn_recalculatePromo').show();
                window.down('#btn_resetPromo').hide();
            } else {
                window.down('#btn_recalculatePromo').hide();
                window.down('#btn_resetPromo').hide();
            }
        }

        // если расчеты закончились, необходимо обновить форму
        window.setLoading(l10n.ns('core').value('savingText'));

        App.model.tpm.promo.Promo.load(record.get('Id'), {
            callback: function (newModel, operation, success) {
                if (success) {
                    var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                    var directorygrid = grid ? grid.down('directorygrid') : null;
                    var currentRecord = me.getRecord(window);

                    if (currentRecord.get('Calculating')) {
                        currentRecord.data.Calculating = newModel.data.Calculating;
                        window.model = newModel;
                        me.reFillPromoForm(window, newModel, directorygrid);

                        // Если создание из календаря - обновляем календарь
                        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                        if (scheduler) {
                            scheduler.resourceStore.reload();
                            scheduler.eventStore.reload();
                        }
                    }
                }
                else {
                    window.setLoading(false);
                    App.Notify.pushError('Error!');
                }
            }
        });
    },

    colorRaws: function (grid) {
        grid.getView().getRowClass = function (record, rowIndex, rowParams, store) {
            if (record.data.Type === 'WARNING') {
                return 'warningraw'
            } else if (record.data.Type === 'ERROR') {
                return 'errorraw'
            } else {
                return ''
            }
        }
    },

    // т.к. Форма редактирования кастомная, нет необходимости её обновлять
    updateDetailForm: function () { },

    // Показывать промо для которых возможно изменение статуса под текущей ролью
    onShowEditableButtonClick: function (button) {
        var store = this.getGridByButton(button).getStore(),
            proxy = store.getProxy()
        proxy.extraParams.canChangeStateOnly = !proxy.extraParams.canChangeStateOnly;

        store.clearData();
        store.load();

        if (proxy.extraParams.canChangeStateOnly) {
            button.addCls('showEditablePromo-btn-active');
        } else {
            button.removeCls('showEditablePromo-btn-active');
        }
    },

    onPromoBudgetsDetailsWindowAfterRender: function (window) {
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

        // Plan - Total Cost Budgets
        window.down('[name=PlanPromoCost]').setValue(record.data.PlanPromoCost);
        window.down('[name=PlanPromoTIMarketing]').setValue(record.data.PlanPromoTIMarketing);
        window.down('[name=PlanPromoCostProduction]').setValue(record.data.PlanPromoCostProduction);
        window.down('[name=PlanPromoTIShopper]').setValue(record.data.PlanPromoTIShopper);
        window.down('[name=PlanPromoBranding]').setValue(record.data.PlanPromoBranding);
        window.down('[name=PlanPromoBTL]').setValue(record.data.PlanPromoBTL);

        // Plan - Marketing TI
        window.down('[name=budgetDet-PlanX-sites]').setValue(record.data.PlanPromoXSites);
        window.down('[name=budgetDet-PlanCatalog]').setValue(record.data.PlanPromoCatalogue);
        window.down('[name=budgetDet-PlanPOSM]').setValue(record.data.PlanPromoPOSMInClient);

        // Plan - Cost Production
        window.down('[name=budgetDet-PlanCostProdX-sites]').setValue(record.data.PlanPromoCostProdXSites);
        window.down('[name=budgetDet-PlanCostProdCatalog]').setValue(record.data.PlanPromoCostProdCatalogue);
        window.down('[name=budgetDet-PlanCostProdPOSM]').setValue(record.data.PlanPromoCostProdPOSMInClient);

        // Actual - Total Cost Budgets
        window.down('[name=ActualPromoCost]').setValue(record.data.ActualPromoCost);
        window.down('[name=ActualPromoTIMarketing]').setValue(record.data.ActualPromoTIMarketing);
        window.down('[name=ActualPromoCostProduction]').setValue(record.data.ActualPromoCostProduction);
        window.down('[name=ActualPromoTIShopper]').setValue(record.data.ActualPromoTIShopper);
        window.down('[name=ActualPromoBranding]').setValue(record.data.ActualPromoBranding);
        window.down('[name=ActualPromoBTL]').setValue(record.data.ActualPromoBTL);

        // Actual - Marketing TI
        window.down('[name=budgetDet-ActualX-sites]').setValue(record.data.ActualPromoXSites);
        window.down('[name=budgetDet-ActualCatalog]').setValue(record.data.ActualPromoCatalogue);
        window.down('[name=budgetDet-ActualPOSM]').setValue(record.data.ActualPromoPOSMInClient);

        // Actual - Cost Production
        window.down('[name=budgetDet-ActualCostProdX-sites]').setValue(record.data.ActualPromoCostProdXSites);
        window.down('[name=budgetDet-ActualCostProdCatalog]').setValue(record.data.ActualPromoCostProdCatalogue);
        window.down('[name=budgetDet-ActualCostProdPOSM]').setValue(record.data.ActualPromoCostProdPOSMInClient);
    },

    onPromoFinanceDetailsWindowAfterRender: function (window) {
        var promoController = App.app.getController('tpm.promo.Promo'),
            record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

        window.down('[name=PlanPromoNetIncrementalLSV]').setValue(record.data.PlanPromoNetIncrementalLSV);
        window.down('[name=PlanPromoNetLSV]').setValue(record.data.PlanPromoNetLSV);
        window.down('[name=PlanPromoNetIncrementalNSV]').setValue(record.data.PlanPromoNetIncrementalNSV);
        window.down('[name=PlanPromoPostPromoEffectLSV]').setValue(record.data.PlanPromoPostPromoEffectLSV);
        window.down('[name=PlanPromoNetNSV]').setValue(record.data.PlanPromoNetNSV);
        window.down('[name=PlanPromoNetIncrementalEarnings]').setValue(record.data.PlanPromoNetIncrementalEarnings);
        window.down('[name=PlanPromoNetROIPercent]').setValue(record.data.PlanPromoNetROIPercent);

        window.down('[name=ActualPromoNetIncrementalLSV]').setValue(record.data.ActualPromoNetIncrementalLSV);
        window.down('[name=ActualPromoNetLSV]').setValue(record.data.ActualPromoNetLSV);
        window.down('[name=ActualPromoNetIncrementalNSV]').setValue(record.data.ActualPromoNetIncrementalNSV);
        window.down('[name=ActualProductPostPromoEffectLSV]').setValue(record.data.ActualPromoPostPromoEffectLSV);
        window.down('[name=ActualPromoNetNSV]').setValue(record.data.ActualPromoNetNSV);
        window.down('[name=ActualPromoNetIncrementalEarnings]').setValue(record.data.ActualPromoNetIncrementalEarnings);
        window.down('[name=ActualPromoNetROIPercent]').setValue(record.data.ActualPromoNetROIPercent);
    },

    onPromoActivityDetailsWindowAfterRender: function (window) {
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

        var actualInStoreMechanicId = window.down('[name=ActualInstoreMechanicId]');
        var actualInStoreMechanicTypeId = window.down('[name=ActualInstoreMechanicTypeId]');

        // Instore Assumption
        actualInStoreMechanicId.setValue(new App.model.tpm.mechanic.Mechanic({
            Id: record.data.ActualInStoreMechanicId,
            Name: record.data.ActualInStoreMechanicName
        }));

        actualInStoreMechanicTypeId.setValue(new App.model.tpm.mechanictype.MechanicType({
            Id: record.data.ActualInStoreMechanicTypeId,
            Name: record.data.PlanInstoreMechanicTypeName
        }));

        window.down('[name=ActualInStoreDiscount]').setValue(record.data.ActualInStoreDiscount == 0 ? null : record.data.ActualInStoreDiscount);

        window.down('[name=PlanInstoreMechanicIdInActivity]').setRawValue(record.data.PlanInstoreMechanicName);
        window.down('[name=PlanInstoreMechanicTypeIdInActivity]').setRawValue(record.data.PlanInstoreMechanicTypeName);
        window.down('[name=PlanInstoreMechanicDiscountInActivity]').setRawValue(record.data.PlanInstoreMechanicDiscount);

        // Plan - Activity
        window.down('[name=PlanPromoUpliftPercent]').setValue(record.data.PlanPromoUpliftPercent);
        window.down('[name=PlanPromoBaselineLSV]').setValue(record.data.PlanPromoBaselineLSV);
        window.down('[name=PlanPromoIncrementalLSV]').setValue(record.data.PlanPromoIncrementalLSV);
        window.down('[name=PlanPromoLSV]').setValue(record.data.PlanPromoLSV);
        window.down('[name=PlanPromoPostPromoEffectLSV]').setValue(record.data.PlanPromoPostPromoEffectLSV);

        // In Store Shelf Price
        window.down('[name=ActualInStoreShelfPrice]').setValue(record.data.ActualInStoreShelfPrice);
        window.down('[name=PlanInStoreShelfPrice]').setValue(record.data.PlanInStoreShelfPrice);

        // Actual - Activityasa
        window.down('[name=SumInvoice]').setValue(record.data.SumInvoice);
        window.down('[name=InvoiceNumber]').setValue(record.data.InvoiceNumber);
        window.down('[name=DocumentNumber]').setValue(record.data.DocumentNumber);
        window.down('[name=ActualPromoUpliftPercent]').setValue(record.data.ActualPromoUpliftPercent);
        window.down('[name=ActualPromoBaselineLSV]').setValue(record.data.ActualPromoBaselineLSV);
        window.down('[name=ActualPromoIncrementalLSV]').setValue(record.data.ActualPromoIncrementalLSV);
        window.down('[name=ActualPromoLSVByCompensation]').setValue(record.data.ActualPromoLSVByCompensation);
        window.down('[name=ActualPromoLSV]').setValue(record.data.ActualPromoLSV);
        window.down('[name=ActualPromoLSVSO]').setValue(record.data.ActualPromoLSVSO);
        window.down('[name=ActualPromoPostPromoEffectLSV]').setValue(record.data.ActualPromoPostPromoEffectLSV);
    },

    onPromoProductSubrangeDetailsWindowAfterRender: function (window) {
        var record = this.getRecord(window);
        var productTreeGrid = Ext.widget('producttreegrid', {
            disabled: true,
            cls: 'template-tree scrollpanel promo-product-window',
            store: {
                model: 'App.model.tpm.producttree.ProductTree',
                autoLoad: false,
                root: {},
                storeId: 'TempProductTree'
            },
        });

        var store = productTreeGrid.getStore();
        // даже для разных сторов proxy один
        var proxy = store.getProxy();
        var oldExtraParams = {
            dateFilter: proxy.extraParams.dateFilter,
            filterParameter: proxy.extraParams.filterParameter,
            productTreeObjectIds: proxy.extraParams.productTreeObjectIds,
            promoId: proxy.extraParams.promoId,
            view: proxy.extraParams.view
        };

        store.on({
            load: {
                fn: function () {
                    var productTreeController = App.app.getController('tpm.product.ProductTree');
                    var trueRoot = store.getById('1000000');

                    store.setRootNode(trueRoot);
                    productTreeController.setCheckTree(store.getRootNode().childNodes, true);
                    store.getProxy().extraParams = oldExtraParams; // возвращаем параметры в исходную
                    Ext.ComponentQuery.query('#promo_step2')[0].down('producttreegrid').unmask();
                },
                single: true
            }
        });

        proxy.extraParams.dateFilter = null;
        proxy.extraParams.promoId = record.get('Id');
        proxy.extraParams.view = true;
        store.load();

        window.down('#containerForProductTreeGrid').add(productTreeGrid);
    },
    // сбрасываем параметр показывающий только промо зависящие от текущего пользователя при закрытии панели
    onPromoGridBeforeDestroy: function (panel) {
        var store = panel.down('directorygrid').getStore(),
            proxy = store.getProxy();
        proxy.extraParams.canChangeStateOnly = false;
        return true;
    },

    //фунция для определения положения ползунка, чтобы после закрытия окна промо и перезагрузки стора прокрутить на прежнее место
    getScrollPosition: function (button) {
        if (button) {
            var grid = button.up('combineddirectorypanel').down('directorygrid'),
                view = grid.getView(),
                positionY = $(view.getEl().dom).data('jsp').getContentPositionY();

            view.positionY = positionY;
        }
    },

    initSignalR: function (window) {
        var record = this.getRecord(window);
        var promoId = record.get('Id');
        var blocked = record.get('Calculating');

        requestHub($.connection.logHub.server.subscribeStatus, [promoId, blocked]);
        window.down('#btn_showlog').setDisabled(false);
    },

    recalculatePromo: function (btn) {
        var window = btn.up('promoeditorcustom');
        var record = this.getRecord(window);
        var promoId = record.get('Id');
        var me = this;

        window.isChanged = true;
        window.setLoading(l10n.ns('core').value('savingText'));

        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/RecalculatePromo?promoId=' + promoId,
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);

                if (data.success) {
                    // привет EXT!
                    App.model.tpm.promo.Promo.load(promoId, {
                        callback: function (newModel, operation) {
                            var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                            if (window && !window.isDestroyed) {
                                var directorygrid = grid ? grid.down('directorygrid') : null;

                                window.promoId = data.Id;
                                window.model = newModel;
                                me.reFillPromoForm(window, newModel, directorygrid);
                                me.updateStatusHistoryState();
                                //24.06.19 Лог не показываем
                                //if (newModel.get('Calculating'))
                                //    me.onPrintPromoLog(window, grid, close);
                            }
                        }
                    });
                }
                else {
                    window.setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedLoadData'));
                }

            },
            error: function (data) {
                window.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },

    // --------------------- Выбор клиента -----------------//
    // поспотреть параметры клиента
    onPromoClientSettignsClick: function (button) {
        var promoClientForm = button.up('promoclient');

        promoClientForm.showSettings();
    },

    // событие нажатия кнопки выбора клиента
    onChoosePromoClientClick: function (button) {
        var promoClientForm = button.up('promoclient');
        var me = this;

        var promoeditorcustom1 = promoClientForm.up('promoeditorcustom');
        var record = this.getRecord(promoeditorcustom1);
        if (record.ClientHierarchyFilterDate) {
            promoClientForm.promoClientForm.treesChangingBlockDate = record.ClientHierarchyFilterDate;
        }
        // передаем также call-back функцию, отвечающую за dispatch
        // также устанавливаем подписи кнопок (Promo Basic Steps)
        promoClientForm.chooseClient(function (clientTreeRecord) {
            var promoeditorcustom = promoClientForm.up('promoeditorcustom');
            var clientTreeRecordRaw = clientTreeRecord ? clientTreeRecord.raw : null;

            me.checkParametersAfterChangeClient(clientTreeRecordRaw, promoeditorcustom);
            me.refreshPromoEvent(promoeditorcustom, true);

            //Разрешаем выбор механики, когда выбран клиент, а при смене клиента очищаем поля
            if (clientTreeRecordRaw !== null) {
                var promoActivityMechanic = promoeditorcustom.down('container[name=promoActivity_step1]');
                var promoMechanics = promoeditorcustom.down('promomechanic');
                var marsMechanicId = promoMechanics.down('[name=MarsMechanicId]');
                var instoreMechanicId = promoMechanics.down('[name=PlanInstoreMechanicId]');
                var actualMechanicId = promoActivityMechanic.down('[name=ActualInstoreMechanicId]');

                marsMechanicId.reset();
                instoreMechanicId.reset();
                actualMechanicId.reset();

                marsMechanicId.setDisabled(false);
                instoreMechanicId.setDisabled(false);
                actualMechanicId.setDisabled(false);
            }
        });

        var promoClientChooseWindow = Ext.ComponentQuery.query('promoclientchoosewindow')[0];

        var dateFilter = promoClientChooseWindow.down('#dateFilter');
        var currentDate = new Date();

        var days = currentDate.getDate().toString().length === 1 ? '0' + currentDate.getDate() : currentDate.getDate();
        var month = (currentDate.getMonth() + 1).toString().length === 1 ? '0' + (currentDate.getMonth() + 1) : currentDate.getMonth() + 1;
        var year = currentDate.getFullYear();

        if (dateFilter) {
            dateFilter.setText(days + '.' + month + '.' + year);
        }

        // Если нет выбранных базовых клиентов включаем кнопку Choose
        if (promoClientChooseWindow.choosenClientObjectId === null /*|| isAnyChecked*/) {
            var chooseButton = promoClientChooseWindow.down('#choose');
            chooseButton.setDisabled(true);
        }

        var baseClientChBox = promoClientChooseWindow.down('#baseClientsCheckbox');
        baseClientChBox.suspendEvents();
        baseClientChBox.setValue(true);
        baseClientChBox.resumeEvents();
    },

    //onClientChooseWindowAfterrender: function (window) {

    //	var isAnyChecked = false;
    //	var nodes = window.down('treeview');
    //	if (nodes) {
    //		nodes.all.elements.forEach(function (el) {
    //			if (el.classList.indexOf('x-grid-row-selected') !== -1) {
    //				isAnyChecked = true;
    //			}
    //		});
    //	}

    //},

    // обработчик выбора клиента в форме выбора клиента
    onClientTreeCheckChange: function (item, checked, eOpts) {
        var treegrid = item.store.ownerTree;
        var nodes = treegrid.getRootNode().childNodes;
        var clientTree = treegrid.up('clienttree');
        var editorForm = clientTree.down('editorform');
        var clientTreeController = this.getController('tpm.client.ClientTree');
        var promoClientChooseWindow = Ext.ComponentQuery.query('promoclientchoosewindow')[0];
        var chooseButton = promoClientChooseWindow.down('#choose');
        clientTreeController.setCheckTree(nodes, false);

        if (checked) {
            item.set('checked', true);
            clientTree.choosenClientObjectId = item.get('ObjectId');
            clientTreeController.updateTreeDetail(editorForm, item);

            if (clientTree.choosenClientObjectId !== null) {
                chooseButton.setDisabled(false);
            }

            var store = item.store;
            var parent = store.getNodeById(item.get('parentId'));

            if (parent) {
                while (parent.data.root !== true) {
                    if (parent.get('IsBaseClient')) {
                        parent.set('checked', true);
                    }
                    parent = store.getNodeById(parent.get('parentId'));
                }
            }
        }
        else {
            clientTree.choosenClientObjectId = null;
            chooseButton.setDisabled(true);
        }
    },

    // При нажатии на выбор даты в окне выбра клиента
    onClientDateFilterButtonClick: function (button) {
        var me = this;
        var datetimeField = Ext.widget('datetimefield');
        datetimeField.dateFormat = 'd.m.Y';
        var datetimePicker = Ext.widget('datetimepicker', {
            pickerField: datetimeField,
            title: datetimeField.selectorTitle,
            width: datetimeField.selectorWidth,
            height: datetimeField.selectorHeight,
            value: button.dateValue || new Date(),
            listeners: {
                scope: datetimeField,
                select: datetimeField.onSelect
            },
            increment: datetimeField.increment
        });

        datetimePicker.show();

        var promoclientchoosewindow = button.up('promoclientchoosewindow');
        var okButton = Ext.ComponentQuery.query('button[action="ok"]')[0];
        var dateFilterButton = Ext.ComponentQuery.query('#dateFilter')[0];
        var clientTreeGrid = promoclientchoosewindow.down('clienttreegrid');
        var store = clientTreeGrid.store;

        okButton.addListener('click', function () {
            var resultDate = datetimeField.getValue();
            var days = resultDate.getDate().toString().length === 1 ? '0' + resultDate.getDate() : resultDate.getDate();
            var month = (resultDate.getMonth() + 1).toString().length === 1 ? '0' + (resultDate.getMonth() + 1) : resultDate.getMonth() + 1;
            var year = resultDate.getFullYear();
            button.dateValue = resultDate;
            dateFilterButton.setText(days + '.' + month + '.' + year);
            store.getProxy().extraParams.dateFilter = resultDate;
            me.applyFiltersForClientTree(promoclientchoosewindow);
        });
    },

    applyFiltersForClientTree: function (promoclientchoosewindow) {
        var textFieldSearch = promoclientchoosewindow.down('#clientsSearchTrigger');
        var baseClientChBox = promoclientchoosewindow.down('#baseClientsCheckbox');
        var store = promoclientchoosewindow.down('basetreegrid').store;
        var proxy = store.getProxy();
        var me = this;

        var textSearch = textFieldSearch.getValue()
        if (textSearch && textSearch.length > 0 && textSearch.indexOf('Client search') == -1)
            proxy.extraParams.filterParameter = textSearch;

        proxy.extraParams.needBaseClients = baseClientChBox.getValue();

        if (promoclientchoosewindow.choosenClientObjectId)
            proxy.extraParams.clientObjectId = promoclientchoosewindow.choosenClientObjectId;

        store.getRootNode().removeAll();
        store.getRootNode().setId('root');
        store.load();

        proxy.extraParams.filterParameter = null;
        proxy.extraParams.needBaseClients = false;
        proxy.extraParams.clientObjectId = null;
    },

    // При нажатии на выбор даты в окне выбра продуктов
    onProductDateFilterButtonClick: function (button) {
        var me = this;
        var datetimeField = Ext.widget('datetimefield');
        datetimeField.dateFormat = 'd.m.Y';
        var datetimePicker = Ext.widget('datetimepicker', {
            pickerField: datetimeField,
            title: datetimeField.selectorTitle,
            width: datetimeField.selectorWidth,
            height: datetimeField.selectorHeight,
            value: button.dateValue || new Date(),
            listeners: {
                scope: datetimeField,
                select: datetimeField.onSelect
            },
            increment: datetimeField.increment
        });

        datetimePicker.show();

        var promoproductchoosewindow = button.up('promoproductchoosewindow');
        var okButton = Ext.ComponentQuery.query('button[action="ok"]')[0];
        var dateFilterButton = Ext.ComponentQuery.query('#dateFilter')[0];
        var productTreeGrid = promoproductchoosewindow.down('producttreegrid');
        var store = productTreeGrid.store;

        okButton.addListener('click', function () {
            var resultDate = datetimeField.getValue();
            var days = resultDate.getDate().toString().length === 1 ? '0' + resultDate.getDate() : resultDate.getDate();
            var month = (resultDate.getMonth() + 1).toString().length === 1 ? '0' + (resultDate.getMonth() + 1) : resultDate.getMonth() + 1;
            var year = resultDate.getFullYear();
            button.dateValue = resultDate;
            dateFilterButton.setText(days + '.' + month + '.' + year);
            store.getProxy().extraParams.dateFilter = resultDate;
            me.applyFiltersForProductTree(promoproductchoosewindow);
        });
    },

    applyFiltersForProductTree: function (promoproductchoosewindow) {
        var textFieldSearch = promoproductchoosewindow.down('#productsSearchTrigger');
        var store = promoproductchoosewindow.down('basetreegrid').store;
        var proxy = store.getProxy();

        var textSearch = textFieldSearch.getValue()
        if (textSearch && textSearch.length > 0 && textSearch.indexOf('Product search') == -1)
            proxy.extraParams.filterParameter = textSearch;

        if (promoproductchoosewindow.choosenProductObjectIds.length > 0) {
            promoproductchoosewindow.choosenProductObjectIds.forEach(function (objectId, index) {
                proxy.extraParams.productTreeObjectIds += objectId;

                if (index != promoproductchoosewindow.choosenProductObjectIds.length - 1)
                    proxy.extraParams.productTreeObjectIds += ';';
            });
        }
        else {
            proxy.extraParams.productTreeObjectIds = null;
        }

        store.getRootNode().removeAll();
        store.getRootNode().setId('root');
        store.load();
        proxy.extraParams.filterParameter = null;
    },

    // при нажатии кнопки выбрать в форме выбора клиента
    onChooseClientTreeClick: function (button) {
        var promoClientChooseWnd = button.up('promoclientchoosewindow');
        var clientTree = promoClientChooseWnd.down('clienttree');

        // если сменился клиент и есть call-back функция, то вызываем её, возвращая выбранного клиента
        if (promoClientChooseWnd.choosenClientObjectId != clientTree.choosenClientObjectId && promoClientChooseWnd.callBackChooseFnc) {
            var clientTreeGrid = clientTree.down('clienttreegrid');
            var clientTreeStore = clientTreeGrid.getStore();
            var clientTreeRecord = clientTreeStore.getById(clientTree.choosenClientObjectId);

            promoClientChooseWnd.callBackChooseFnc(clientTreeRecord);
        }

        promoClientChooseWnd.close();
    },

    // устанавливаем информацию на кнопке Step1 Basic Promo
    // а также меняем клиента в записи окна
    setInfoPromoBasicStep1: function (promoClientForm) {
        var promoWindow = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var clientButton = promoWindow.down('button#btn_promo_step1');

        // если клиент выбран, то clientTreeRecord != null
        if (promoClientForm.clientTreeRecord) {
            var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b><br><p>';
            var displayHierarchy = promoClientForm.clientTreeRecord.FullPathName.replace(/>/g, '<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7"> </span>');

            promoWindow.clientHierarchy = promoClientForm.clientTreeRecord.FullPathName;
            promoWindow.clientTreeId = promoClientForm.clientTreeRecord.ObjectId;
            if (promoClientForm.clientTreeRecord.InOutId !== null && promoClientForm.clientTreeRecord.InOutId !== undefined /*&& (promoClientForm.clientTreeRecord.Id - 10002 > 0)*/) {
                promoWindow.clientTreeKeyId = promoClientForm.clientTreeRecord.InOutId.substr(0, promoClientForm.clientTreeRecord.InOutId.indexOf('-'));
            } else {
                promoWindow.clientTreeKeyId = promoClientForm.clientTreeRecord.Id;
            }

            clientButton.setText(text + displayHierarchy + '</p>');
            clientButton.removeCls('notcompleted');
            clientButton.setGlyph(0xf133);
            clientButton.isComplete = true;
        }
        else {
            promoWindow.clientHierarchy = null;
            promoWindow.clientTreeId = null;
            promoWindow.clientTreeKeyId = null;
            clientButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b>');
            clientButton.addCls('notcompleted');
            clientButton.setGlyph(0xf130);
            clientButton.isComplete = false;

            // dispatch
            var promoperiod = promoWindow.down('promoperiod');
            var periodButton = promoWindow.down('button#btn_promo_step4');
            var dispatchStartDate = promoperiod.down('[name=DispatchStartDate]');
            var dispatchEndDate = promoperiod.down('[name=DispatchEndDate]');
            var durationStartDate = promoperiod.down('[name=DurationStartDate]');
            var durationEndDate = promoperiod.down('[name=DurationEndDate]');

            dispatchStartDate.reset();
            dispatchEndDate.reset();

            var durationPeriod = durationStartDate.getValue() && durationEndDate.getValue() ? 'c ' + Ext.Date.format(durationStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(durationEndDate.getValue(), "d.m.Y") : '';
            var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' + durationPeriod + '<br>Dispatch: ' + '</p>';

            var fieldSetDispatch = dispatchStartDate.up('fieldset');
            fieldSetDispatch.setTitle('Dispatch (0 days)');

            periodButton.setText(text);
            periodButton.addCls('notcompleted');
            periodButton.setGlyph(0xf130);
        }

        // проверяем готовность Promo Basic
        checkMainTab(clientButton.up(), promoWindow.down('button[itemId=btn_promo]'));
    },

    // следующие две функции старые, управляют dispatch
    checkParametersAfterChangeClient: function (clientTreeRecord, promoeditorcustom) {
        var me = this;
        var promoClientForm = promoeditorcustom.down('promoclient');
        promoClientForm.fillForm(clientTreeRecord);

        var periodButton = promoeditorcustom.down('button#btn_promo_step4');
        var promoperiod = promoeditorcustom.down('promoperiod');
        var dispatchStartDate = promoperiod.down('[name=DispatchStartDate]');
        var dispatchEndDate = promoperiod.down('[name=DispatchEndDate]');
        var durationStartDate = promoeditorcustom.down('[name=DurationStartDate]');
        var durationEndDate = promoeditorcustom.down('[name=DurationEndDate]');
        var calculation = Ext.ComponentQuery.query('promocalculation')[0];
        var activity = Ext.ComponentQuery.query('promoactivity')[0];
        if (calculation && activity) {
            var planPostPromoEffectFromCalculation = calculation.down('trigger[name=PlanPostPromoEffect]');
            var changePromoButton = promoeditorcustom.down('#changePromo');
            var model = this.getRecord(promoeditorcustom);

            model.data.PlanPostPromoEffectW1 = clientTreeRecord.PostPromoEffectW1 || 0;
            model.data.PlanPostPromoEffectW2 = clientTreeRecord.PostPromoEffectW2 || 0;
            model.data.PlanPostPromoEffect = (clientTreeRecord.PostPromoEffectW1 + clientTreeRecord.PostPromoEffectW2) || 0;

            if (changePromoButton) {
                if (changePromoButton.hidden === true) {
                    planPostPromoEffectFromCalculation.setValue(model.data.PlanPostPromoEffect);
                }
            }
        }

        if (durationStartDate.getValue() && durationEndDate.getValue()) {
            dispatchStartDate.reset();
            dispatchEndDate.reset();

            if (clientTreeRecord) {
                var isBeforeStart = clientTreeRecord.IsBeforeStart;
                var daysStart = clientTreeRecord.DaysStart;
                var isDaysStart = clientTreeRecord.IsDaysStart;
                var daysForDispatchStart = clientTreeRecord.DaysStart;
                var isBeforeEnd = clientTreeRecord.IsBeforeEnd;
                var daysEnd = clientTreeRecord.DaysEnd;
                var isDaysEnd = clientTreeRecord.IsDaysEnd;
                var daysForDispatchEnd = clientTreeRecord.DaysEnd;

                if (isBeforeStart !== null && daysStart !== null && isDaysStart !== null) {
                    var resultDateForDispatchStart = null;

                    if (!isDaysStart) {
                        daysForDispatchStart *= 7;
                    }

                    if (isBeforeStart) {
                        resultDateForDispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY, -daysForDispatchStart);
                    } else {
                        resultDateForDispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY, daysForDispatchStart);
                    }

                    if (resultDateForDispatchStart) {
                        dispatchStartDate.setValue(resultDateForDispatchStart);
                        dispatchEndDate.setMinValue(dispatchStartDate.getValue())
                    }
                }

                if (isBeforeEnd !== null && daysEnd !== null && isDaysEnd !== null) {
                    var resultDateForDispatchEnd = null;

                    if (!isDaysEnd) {
                        daysForDispatchEnd *= 7;
                    }

                    if (isBeforeEnd) {
                        resultDateForDispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY, -daysForDispatchEnd);
                    } else {
                        resultDateForDispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY, daysForDispatchEnd);
                    }

                    if (resultDateForDispatchEnd) {
                        dispatchEndDate.setValue(resultDateForDispatchEnd);
                    }
                }

                if (!dispatchStartDate.getValue() || !dispatchEndDate.getValue()) {
                    periodButton.addCls('notcompleted');
                    periodButton.setGlyph(0xf130);
                }

                var durationPeriod = 'c ' + Ext.Date.format(durationStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(durationEndDate.getValue(), "d.m.Y"),
                    dispatchPeriod = dispatchStartDate.getValue() && dispatchEndDate.getValue() ? 'c ' + Ext.Date.format(dispatchStartDate.getValue(), "d.m.Y") + ' по ' + Ext.Date.format(dispatchEndDate.getValue(), "d.m.Y") : '',
                    text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' + durationPeriod + '<br>Dispatch: ' + dispatchPeriod + '</p>';

                periodButton.setText(text);
            }
        }

        me.setInfoPromoBasicStep1(promoClientForm);
    },

    afterInitClient: function (promoClientRecord, promoRecord, window, isCopy) {
        // Legacy code
        if (promoClientRecord) {
            var schedulerController = this.getController('tpm.schedule.SchedulerViewController');
            var dispatchStart = null;
            var dispatchEnd = null;
            var daysForDispatchStart = null;
            var daysForDispatchEnd = null;
            var period = window.down('container[name=promo_step4]');
            var dispatchStartDate = period.down('datefield[name=DispatchStartDate]');
            var dispatchEndDate = period.down('datefield[name=DispatchEndDate]');

            // period
            var durationStartDate = period.down('datefield[name=DurationStartDate]');
            var durationEndDate = period.down('datefield[name=DurationEndDate]');

            /// Сомнительный участок
            daysForDispatchStart = schedulerController.getDaysForDispatchDateFromClientSettings(
                promoClientRecord.IsBeforeStart, promoClientRecord.DaysStart, promoClientRecord.IsDaysStart);

            daysForDispatchEnd = schedulerController.getDaysForDispatchDateFromClientSettings(
                promoClientRecord.IsBeforeEnd, promoClientRecord.DaysEnd, promoClientRecord.IsDaysEnd);

            if (daysForDispatchStart !== null) {
                dispatchStart = Ext.Date.add(durationStartDate.getValue(), Ext.Date.DAY,
                    schedulerController.getDaysForDispatchDateFromClientSettings(
                        promoClientRecord.IsBeforeStart, promoClientRecord.DaysStart, promoClientRecord.IsDaysStart));
            }

            if (daysForDispatchEnd !== null) {
                dispatchEnd = Ext.Date.add(durationEndDate.getValue(), Ext.Date.DAY,
                    schedulerController.getDaysForDispatchDateFromClientSettings(
                        promoClientRecord.IsBeforeEnd, promoClientRecord.DaysEnd, promoClientRecord.IsDaysEnd));

            }

            if (isCopy) {
                // Берутся настройки dispatch у клиента, на которого копируется промо в календаре
                dispatchStartDate.setValue(dispatchStart);
                dispatchEndDate.setValue(dispatchEnd);
            } else {
                // Берутся настройки dispatch у текущей записи, в том числе при открытии на редактирование и просмотр.
                // Наличие или отсутсвие dispatch настроек у клиента не влияет на это действие.
                //При наличии dispatch настроек у клиента при создании нет данных в promoRecord.data.DispatchesStart,и соответсвненно нет dispatch дат
                if (promoRecord.data.DispatchesStart != undefined && promoRecord.data.DispatchesStart != undefined) {
                    dispatchStartDate.setValue(promoRecord.data.DispatchesStart);
                    dispatchEndDate.setValue(promoRecord.data.DispatchesEnd);
                } else {
                    if (dispatchStart != null) {
                        dispatchStartDate.setValue(dispatchStart);

                    } if (dispatchEnd != null) {
                        dispatchEndDate.setValue(dispatchEnd);

                    }
                }

            }

            var promoPeriod = window.down('promoperiod');
            var durationStartDate = promoPeriod.down('datefield[name=DurationStartDate]');
            var durationEndDate = promoPeriod.down('datefield[name=DurationEndDate]');

            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);

            // обновление информации Event
            var me = this;
            me.refreshPromoEvent(window, false);
        }

    },


    // --------------------- Выбор события -----------------//

    refreshPromoEvent: function (promoeditorcustom, needClearEvent) {
        var me = this;
        var promoEditorCustom = promoeditorcustom;
        var record = me.getRecord(promoEditorCustom);
        var promoEvent = promoEditorCustom.down('container[name=promo_step6]');
        var chooseEventButton = promoEvent.down('chooseEventButton');
        var clientTreeKeyId = promoEditorCustom.clientTreeKeyId;
        
        // при каждом вызове этой функции Event сбрасывается до стандартного (в дальнейшем желательно сделать проверку на возможность оставить предзаполненный Event)
        var _event = new App.model.tpm.event.Event({
            Id: null,
            Name: 'Standard promo',
            Description: ''
        });

        // если не требуется стандартный Event назначить выбранный Event
        if (!needClearEvent && record.data.EventId) {
            _event = new App.model.tpm.event.Event({
                Id: record.data.EventId,
                Name: record.data.PromoEventName,
                Description: record.data.PromoEventDescription
            })
        };

        chooseEventButton.setValue(_event);
        chooseEventButton.updateMappingValues(_event);

        if (clientTreeKeyId !== null) {
            promoEvent.setDisabled(false);
        }
        else {
            promoEvent.setDisabled(true);
        }
        chooseEventButton.clientTreeKeyId = clientTreeKeyId;
    },

    // --------------------- Выбор продуктов -----------------//

    // событие нажатия кновки выбора продуктов
    onChoosePromoProductsBtnClick: function (button) {
        var promoEditorCustom = button.up('promoeditorcustom');

        if (promoEditorCustom.isInOutPromo || (promoEditorCustom.model && promoEditorCustom.model.data.InOut)
            || (promoEditorCustom.assignedRecord && promoEditorCustom.assignedRecord.data.InOut)) {
            promoEditorCustom.setLoading(true);
            setTimeout(function () {
                Ext.widget('inoutselectionproductwindow').show(false, function () {
                    promoEditorCustom.setLoading(false);
                });
            });
        } else {
            setTimeout(function () {
                Ext.widget('inoutselectionproductwindow').show(false, function () {
                    promoEditorCustom.setLoading(false);
                });
            });
            //         var me = this;
            //         var promoProductsForm = button.up('promobasicproducts');

            //         promoProductsForm.chooseProducts(function (nodesProductTree) {
            //             promoProductsForm.fillForm(nodesProductTree);
            //             me.setInfoPromoBasicStep2(promoProductsForm);
            //});

            //var promoClientChooseWindow = Ext.ComponentQuery.query('promoproductchoosewindow')[0];
            //var dateFilter = promoClientChooseWindow.down('#dateFilter');
            //var currentDate = new Date();

            //var days = currentDate.getDate().toString().length === 1 ? '0' + currentDate.getDate() : currentDate.getDate();
            //var month = (currentDate.getMonth() + 1).toString().length === 1 ? '0' + (currentDate.getMonth() + 1) : currentDate.getMonth() + 1;
            //var year = currentDate.getFullYear();

            //if (dateFilter) {
            //	dateFilter.setText(days + '.' + month + '.' + year);
            //}

            // Если нет выбранных узлов включаем кнопку Choose
            //var promoProductChooseWindow = Ext.ComponentQuery.query('promoproductchoosewindow')[0];
            //if (promoProductChooseWindow.choosenProductObjectIds.length === 0) {
            //	var chooseButton = promoProductChooseWindow.down('#choose');
            //	chooseButton.setDisabled(true);
            //}
        }
    },

    // событие обработки галочек в дереве продуктов
    onProductTreeCheckChange: function (item, checked, eOpts) {
        var store = item.store;
        var treegrid = store.ownerTree;
        var productTree = treegrid.up('producttree');
        var nodes = treegrid.getRootNode().childNodes;
        var c = this.getController('tpm.product.ProductTree');
        var promoProductChooseWindow = Ext.ComponentQuery.query('promoproductchoosewindow')[0];
        var chooseButton = promoProductChooseWindow.down('#choose');

        // если узел имеет тип subrange, то ищем отмеченные узлы на том же уровне        
        var multiCheck = false;

        if (item.get('Type').indexOf('Subrange') >= 0) {
            for (var i = 0; i < item.parentNode.childNodes.length && !multiCheck; i++) {
                var node = item.parentNode.childNodes[i];
                multiCheck = node !== item && node.get('checked') && node.get('Type').indexOf('Subrange') >= 0;
            }
        }
        // если нельзя отметить несколько, сбрасываем галочки отключаем кноку choose
        if (!multiCheck) {
            c.setCheckTree(nodes, false);
            productTree.choosenClientObjectId = [];
            chooseButton.setDisabled(true);
        }

        if (checked) {
            item.set('checked', true);
            productTree.choosenClientObjectId.push(item.get('ObjectId'));

            // Если есть выбранные узлы включаем кнопку Choose
            if (productTree.choosenClientObjectId.length > 0) {
                chooseButton.setDisabled(false);
            }

            var parent = store.getNodeById(item.get('parentId'));
            if (parent) {
                while (parent.data.root !== true) {
                    if (parent.get('Type').indexOf('Brand') < 0) {
                        parent.set('checked', true);
                    }
                    parent = store.getNodeById(parent.get('parentId'));
                }
            }
        }
        else {
            item.set('checked', false);

            var indexObjectId = productTree.choosenClientObjectId.indexOf(item.data.ObjectId);
            if (indexObjectId >= 0) {
                productTree.choosenClientObjectId.splice(indexObjectId, 1);
            }

            // Если не осталось выбранных узлов выключаем кнопку Choose
            if (productTree.choosenClientObjectId.length === 0) {
                chooseButton.setDisabled(true);
            }
        }

        //Если не начально загруженная запись - заблокировать кнопку productList
        //if (productButton.up('promoeditorcustom').productsSetted) {
        //    var productListButton = producttree.down('#productList');
        //    productListButton.setDisabled(true);
        //}
    },

    // событие нажатия кнопки Choose в окне выбора продуктов
    onChooseProductTreeClick: function (button) {
        var promoProductChooseWnd = button.up('promoproductchoosewindow');
        var productTree = promoProductChooseWnd.down('producttree');

        // если есть call-back функция, то вызываем её, возвращая выбранного клиента
        if (promoProductChooseWnd.callBackChooseFnc) {
            var productTreeGrid = productTree.down('producttreegrid');
            var productTreeStore = productTreeGrid.getStore();
            // выбранные узлы
            var productTreeNodes = [];

            // ищем записи по ObjectId в Store
            productTree.choosenClientObjectId.forEach(function (item) {
                var record = productTreeStore.getById(item);
                productTreeNodes.push(record)
            });

            promoProductChooseWnd.callBackChooseFnc(productTreeNodes);
        }

        promoProductChooseWnd.close();
    },

    // устанавливаем информацию на кнопке Step2 Basic Promo
    // а также меняем продукты в записи окна
    setInfoPromoBasicStep2: function (promoProductForm) {
        var promoWindow = promoProductForm.up('promoeditorcustom');
        var productButton = promoWindow.down('button#btn_promo_step2');

        // если клиент выбран, то clientTreeRecord != null
        if (promoProductForm.choosenProductObjectIds.length > 0 || promoProductForm.promoProductRecord) {
            var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b><br><p>';
            var displayHierarchy = promoProductForm.fullPath.replace(/>/g, '<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7"> </span>');

            productButton.setText(text + displayHierarchy + '</p>');
            productButton.removeCls('notcompleted');
            productButton.setGlyph(0xf133);
            productButton.isComplete = true;
            promoWindow.productTreeNodes = promoProductForm.choosenProductObjectIds;
            promoWindow.productHierarchy = promoProductForm.fullPath;
        }
        else {
            productButton.setText('<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b>');
            productButton.addCls('notcompleted');
            productButton.setGlyph(0xf130);
            productButton.isComplete = false;
            promoWindow.productTreeNodes = [];
            promoWindow.productHierarchy = null;
        }

        // проверяем готовность Promo Basic        
        checkMainTab(productButton.up(), promoWindow.down('button#btn_promo'));
    },

    // простмотр продуктов под выбранные Subranges
    onFilteredListBtnClick: function (button) {
        var promoController = this.getController('tpm.promo.Promo');
        var promoEditorCustom = button.up('promoeditorcustom');
        var record = promoController.getRecord(promoEditorCustom);
        var productlist = Ext.widget('productlist', { title: l10n.ns('tpm', 'PromoBasicProducts').value('SelectedProducts') });
        var grid = productlist.down('directorygrid');

        var productIdsString = promoEditorCustom.InOutProductIds || (promoEditorCustom.model && promoEditorCustom.model.data.InOutProductIds) || record.data.InOutProductIds;
        var productIdsArray = productIdsString.split(';');
        productIdsArray = productIdsArray.filter(function (el) {
            return el != '';
        });

        var prefilter = {
            operator: "or",
            rules: []
        };
        productIdsArray.forEach(function (id) {
            prefilter.rules.push({
                operation: "Equals",
                property: "Id",
                value: id
            });
        });

        grid.getStore().setFixedFilter('PreFilter', prefilter);
        productlist.show();
    },

    // просмотр зафиксированных продуктов
    onProductListBtnClick: function (button) {
        var productlist = Ext.widget('actualproductlist');
        var promogrid = button.up('promoeditorcustom');
        var grid = productlist.down('directorygrid');
        var store = grid.getStore();

        store.setFixedFilter('PromoId', {
            property: 'PromoId',
            operation: 'Equals',
            value: promogrid.promoId
        });

        productlist.show();
    },

    onFilteredListBtnAfterRender: function (button) {
        var promoEditorCustom = button.up('promoeditorcustom');
        var record = this.getRecord(promoEditorCustom);
        if (record.data.InOut || promoEditorCustom.isInOutPromo) {
            button.setText(l10n.ns('tpm', 'PromoBasicProducts').value('SelectedProducts'));
        }
    },

    checkLogForErrors: function (recordId) {
        var parameters = {
            promoId: recordId
        };
        App.Util.makeRequestWithCallback('Promoes', 'CheckIfLogHasErrors', parameters, function (data) {
            var result = Ext.JSON.decode(data.httpResponse.data.value);
            
            var but = Ext.ComponentQuery.query('promoeditorcustom #btn_showlog')[0];
            if (but && !but.isDestroyed) {
                if (result.LogHasErrors) {
                    but.addCls('errorinside');
                } else {
                    but.removeCls('errorinside');
                }
            }
        })
    },

    getGrowthAccelerationWindowLabel: function () {
        var growAccelerationComponent = Ext.ComponentQuery.query('#btn_promoGrowthAcceleration')[0];
        return growAccelerationComponent;
    },

    showGrowthAccelerationWindowLabel: function () {
        var growthAccelerationWindowLabel = this.getGrowthAccelerationWindowLabel();
        if (growthAccelerationWindowLabel) {
            growthAccelerationWindowLabel.show();
        }
    },

    hideGrowthAccelerationWindowLabel: function () {
        var growthAccelerationWindowLabel = this.getGrowthAccelerationWindowLabel();
        if (growthAccelerationWindowLabel) {
            growthAccelerationWindowLabel.hide();
        }
    },

    changeGrowthAccelerationState: function (isGrowthAcceleration) {
        this.onGrowthAccelerationCheckboxChange(null, isGrowthAcceleration);
    },

    onGrowthAccelerationCheckboxChange: function (component, newValue) {
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
        var promoController = App.app.getController('tpm.promo.Promo');

        if (promoEditorCustom && newValue) {
            promoController.showGrowthAccelerationWindowLabel();
            promoEditorCustom.isGrowthAcceleration = true;
        } else {
            promoController.hideGrowthAccelerationWindowLabel();
            promoEditorCustom.isGrowthAcceleration = false;
        }
    },

    onApolloExportCheckboxChange: function (component, newValue) {
        var promoEditorCustom = Ext.ComponentQuery.query('promoeditorcustom')[0];

        if (promoEditorCustom && newValue) {
            promoEditorCustom.isApolloExport = true;
        } else {
            promoEditorCustom.isApolloExport = false;
        }
    },

    onExportButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportXLSX';
        var resource = button.resource || proxy.resourceName;

        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST'
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                App.Notify.pushInfo('Задача экспорта справочника промо успешно создана');
                App.System.openUserTasksPanel()
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    }
});