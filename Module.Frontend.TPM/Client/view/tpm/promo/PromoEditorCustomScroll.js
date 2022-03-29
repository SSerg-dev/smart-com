Ext.define('App.view.tpm.promo.PromoEditorCustomScroll', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoeditorcustom',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),

    cls: 'promoform',
    width: "95%",
    height: "95%",
    minWidth: 800,
    minHeight: 600,

    defaults: {
        flex: 1,
        margin: '10 8 15 15'
    },

    listeners: {
        show: function (wind, eOpts) {
            $(wind.down('panel[name=basicPromo]').getTargetEl().dom).find('.jspContainer').on('scroll', function (e) {
                this.scrollTop = 0;
                return false;
            });
            if (wind.isFromSchedule) {
                var container = wind.down('container[name=promo]');

                container.down('#btn_promo_step1').removeCls('selected');
                container.down('#btn_promo_step3').removeCls('selected');
                container.down('#btn_promo_step4').removeCls('selected');
                container.down('#btn_promo_step5').removeCls('selected');
                container.down('#btn_promo_step6').removeCls('selected');
                container.down('#btn_promo_step7').removeCls('selected');
                container.down('#btn_promo_step8').removeCls('selected');

                var jspData = $(wind.down('panel[name=basicPromo]').getTargetEl().dom).data('jsp');
                var el = $(container.down('#promo_step2').getTargetEl().dom);
                jspData.scrollToElement(el, true, true);
                container.down('#btn_promo_step3').addClass('selected');
            }

            var radios = wind.down('[name=promo_step1]').down('[id=invoiceRadiogroup]').items.items;
            Ext.each(radios, function (r) {
                r.fieldLabelTip.setDisabled(true);
                r.fieldValueTip.setDisabled(true);
            });  
        }
    },

    model: null,
    isCreating: true,

    promoId: null,
    statusId: null,
    promoName: 'Unpublish Promo',
    promoNumber: null,
    promoStatusName: null,
    promoStatusSystemName: null,

    detailTabPanel: null,
    clientHierarchy: null,
    clientTreeId: null, // ObjectId
    clientTreeKeyId: null, // Ключ в таблице
    productHierarchy: null,

    initComponent: function () {
        this.callParent(arguments);

        this.down('container[name=promo]').setVisible(true);
        this.down('container[name=promoBudgets]').setVisible(false);
        this.down('container[name=promoActivity]').setVisible(false);
        this.down('container[name=changes]').setVisible(false);
        this.down('panel[name=summary]').setVisible(false);

        this.down('#btn_promo').addCls('selected');
        this.down('#promo_step1').addCls('selected');
        this.down('#promoBudgets_step1').addCls('selected');
        this.down('#promoActivity_step1').addCls('selected');

        // basic promo
        this.down('#btn_promo_step1').addCls('selected');
        this.down('#btn_promo_step1').addCls('notcompleted');
        this.down('#btn_promo_step2').addCls('notcompleted');
        this.down('#btn_promo_step3').addCls('notcompleted');
        this.down('#btn_promo_step4').addCls('notcompleted');
        this.down('#btn_promo_step5').addCls('notcompleted');
        this.down('#btn_promo_step6').addCls('notcompleted');
        this.down('#btn_promo_step7').addCls('notcompleted');
        this.down('#btn_promo_step8').addCls('notcompleted');

        // promo budgets
        this.down('#btn_promoBudgets_step1').addCls('selected');
        this.down('#btn_promoBudgets_step1').addCls('notcompleted');
        this.down('#btn_promoBudgets_step2').addCls('notaccountable');
        this.down('#btn_promoBudgets_step3').addCls('notaccountable');
        this.down('#btn_promoBudgets_step4').addCls('notaccountable');

        // promo activity
        this.down('#btn_promoActivity_step1').addCls('selected');
        //Необязательный шаг
        this.down('#btn_promoActivity_step1').removeCls('notcompleted');
        this.down('#btn_promoActivity_step1').setGlyph(0xf133);
        this.down('#btn_promoActivity_step2').addCls('notcompleted');

        var promoStore;
        var grid = Ext.getCmp('promoGrid');
        var calendarGrid = Ext.ComponentQuery.query('scheduler');
        if (grid) {
            promoStore = grid.down('directorygrid').getStore();
        }
        else if (calendarGrid.length > 0) {
            promoStore = calendarGrid[0].resourceStore;
        }
        if (promoStore) {
            this.on('close', function () {
                var promoeditorcustom = Ext.ComponentQuery.query('promoeditorcustom')[0];
                var isChanged = promoeditorcustom ? promoeditorcustom.isChanged : false;
                if (promoStore && isChanged == true) {
                    promoStore.load();
                }
            });
        }
    },

    items: [{
        xtype: 'panel',
        layout: 'fit',

        dockedItems: [{
            xtype: 'customtoptoolbar',
            dock: 'top'
        }, {
            xtype: 'customdirectorytoolbar',
            dock: 'left',
        }],

        items: [{
            xtype: 'container',
            cls: 'promowindowcontainer',
            layout: 'fit',

            items: [{
                // promo
                xtype: 'panel',
                name: 'promo',
                itemId: 'promo',
                layout: 'fit',

                dockedItems: [{
                    xtype: 'custompromotoolbar',
                    dock: 'left',
                    items: [{
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step1',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep1') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep1'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step2',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep2') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep2'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step3',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep3') + '</b><br><p>Mars: <br>' + l10n.ns('tpm', 'Promo').value('InstoreAssumption') + ': </p>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep3'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false,
                        //Переустановка isComplete для первичного отображения вкладки Basic
                        isInstoreMechanicsComplete: true,
                        isMarsMechanicsComplete: false,
                        listeners: {
                            glyphchange: function (me, newGlyph, oldGlyph) {
                                var productButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step2]')[0];
                                me.isComplete = newGlyph == 0xf133;
                                if (productButton) {
                                    checkMainTab(productButton.up(), productButton.up('promoeditorcustom').down('button[itemId=btn_promo]'));
                                }
                            }
                        }
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step4',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: <br>Dispatch: </p>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep4'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step5',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep5') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep5'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step6',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep6') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep6'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step7',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep7') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep7'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promo_step8',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep8') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('basicStep8'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }],
                }, {
                    xtype: 'custombottomtoolbar',
                    dock: 'bottom',
                    margin: '5 0 0 0'
                }],

                items: [{
                    xtype: 'panel',
                    itemId: 'basicPromoPanel',
                    stopRefreshScroll: true,
                    name: 'basicPromo',
                    autoScroll: true,
                    cls: 'scrollpanel',
                    bodyStyle: { "background-color": "#ECEFF1" },
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                        pack: 'start'
                    },
                    items: [{
                        xtype: 'container',
                        cls: 'promo-editor-custom-scroll-items',
                        items: [{
                            xtype: 'promoclient',
                            name: 'promo_step1',
                            itemId: 'promo_step1',
                            height: 281 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep1'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promobasicproducts',
                            name: 'promo_step2',
                            itemId: 'promo_step2',
                            height: 227 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep2'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promomechanic',
                            name: 'promo_step3',
                            itemId: 'promo_step3',
                            height: 207 + 35,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep3'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promoperiod',
                            name: 'promo_step4',
                            itemId: 'promo_step4',
                            height: 116 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep4'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promobudgetyear',
                            name: 'promo_step5',
                            itemId: 'promo_step5',
                            height: 106 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep5'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promoevent',
                            name: 'promo_step6',
                            itemId: 'promo_step6',
                            height: 156 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep6'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promosettings',
                            name: 'promo_step7',
                            itemId: 'promo_step7',
                            height: 142 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep7'),
                                cls: 'promo-header-item'
                            }
                        }, {
                            xtype: 'promoadjustment',
                            name: 'promo_step8',
                            itemId: 'promo_step8',
                            height: 293 + 36,
                            header: {
                                title: l10n.ns('tpm', 'promoStap').value('basicStep8'),
                                cls: 'promo-header-item'
                            }
                        }]
                    }],

                    _refreshScroll: this.refreshScroll,

                }]
            }, {
                // promo budgets
                xtype: 'panel',
                name: 'promoBudgets',
                itemId: 'promoBudgets',
                layout: 'fit',
                 dockedItems: [{
                    xtype: 'custompromotoolbar',
                    dock: 'left',
                    items: [{
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promoBudgets_step1',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('promoBudgetsStep1') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep1'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promoBudgets_step2',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('promoBudgetsStep2') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep2'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promoBudgets_step3',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('promoBudgetsStep3') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep3'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promoBudgets_step4',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('promoBudgetsStep4') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep4'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: true
                    }],
                }, {
                    xtype: 'custombottomtoolbar',
                    dock: 'bottom',
                    margin: '5 0 0 0'
                }],

                items: [{
                    xtype: 'panel',
                    itemId: 'promoBudgetsPanel',
                    stopRefreshScroll: true,
                    name: 'promoBudgetsContainer',
                    autoScroll: true,
                    cls: 'scrollpanel',
                    bodyStyle: { "background-color": "#ECEFF1" },
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                        pack: 'start',
                    },
                    items: [{
                        xtype: 'promobudgets'
                    }],

                    _refreshScroll: this.refreshScroll,

                    listeners: {
                        afterrender: function (component, eOpts) {
                            $(component.getTargetEl().dom).on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
                                var panel = Ext.ComponentQuery.query('#promoBudgetsPanel')[0].up();

                                var btnStep1 = panel.down('#btn_promoBudgets_step1');
                                var btnStep2 = panel.down('#btn_promoBudgets_step2');
                                var btnStep3 = panel.down('#btn_promoBudgets_step3');
                                var btnStep4 = panel.down('#btn_promoBudgets_step4');

                                var formStep1 = panel.down('#promoBudgets_step1');
                                var formStep2 = panel.down('#promoBudgets_step2');
                                var formStep3 = panel.down('#promoBudgets_step3');
                                var formStep4 = panel.down('#promoBudgets_step4');

                                if (formStep4.needToSetHeight && isAtTop) {
                                    formStep4.setHeight(panel.getHeight() - 20);
                                    formStep4.needToSetHeight = false;
                                } else {
                                    component._refreshScroll(component);
                                }

                                var h1 = formStep1.getHeight();
                                var h1_2 = h1 + formStep2.getHeight();
                                var h1_2_3 = h1_2 + formStep3.getHeight();
                                var h1_2_3_4 = h1_2_3 + formStep4.getHeight();

                                var _deltaY = scrollPositionY + 100;

                                // Step 1 - Total Cost and Budgets
                                if (_deltaY <= h1) {
                                    btnStep1.addClass('selected');
                                    btnStep2.removeCls('selected');
                                    btnStep3.removeCls('selected');
                                    btnStep4.removeCls('selected');

                                    formStep1.header.addClass('promo-header-item-active');
                                    formStep2.header.removeCls('promo-header-item-active');
                                    formStep3.header.removeCls('promo-header-item-active');
                                    formStep4.header.removeCls('promo-header-item-active');

                                    // Step 2 - Marketing TI Budgets
                                } else if (_deltaY > h1 && _deltaY <= h1_2) {
                                    btnStep1.removeCls('selected');
                                    btnStep2.addClass('selected');
                                    btnStep3.removeCls('selected');
                                    btnStep4.removeCls('selected');

                                    formStep1.header.removeCls('promo-header-item-active');
                                    formStep2.header.addClass('promo-header-item-active');
                                    formStep3.header.removeCls('promo-header-item-active');
                                    formStep4.header.removeCls('promo-header-item-active');

                                    // Step 3 - Cost Production Budgets
                                } else if (_deltaY > h1_2 && _deltaY <= h1_2_3) {
                                    btnStep1.removeCls('selected');
                                    btnStep2.removeCls('selected');
                                    btnStep3.addClass('selected');
                                    btnStep4.removeCls('selected');

                                    formStep1.header.removeCls('promo-header-item-active');
                                    formStep2.header.removeCls('promo-header-item-active');
                                    formStep3.header.addClass('promo-header-item-active');
                                    formStep4.header.removeCls('promo-header-item-active');

                                } else if (_deltaY > h1_2_3 && _deltaY <= h1_2_3_4) {
                                    btnStep1.removeCls('selected');
                                    btnStep2.removeCls('selected');
                                    btnStep3.removeCls('selected');
                                    btnStep4.addClass('selected');

                                    formStep1.header.removeCls('promo-header-item-active');
                                    formStep2.header.removeCls('promo-header-item-active');
                                    formStep3.header.removeCls('promo-header-item-active');
                                    formStep4.header.addClass('promo-header-item-active');

                                } else {
                                    console.log('undefined step');
                                }
                            });

                            var mainTab = component.up('promoeditorcustom').down('button[itemId=btn_promoBudgets]');
                            var stepButtons = component.up('panel[name=promoBudgets]').down('custompromotoolbar');
                            checkMainTab(stepButtons, mainTab);
                        }
                    }
                }]
            }, {
                // promo activity
                xtype: 'panel',
                itemId: 'promoActivity',
                name: 'promoActivity',
                layout: 'fit',
                dockedItems: [{
                    xtype: 'custompromotoolbar',
                    dock: 'left',
                    items: [{
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promoActivity_step1',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('promoActivityStep1') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('promoActivityStep1'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false
                    }, {
                        xtype: 'button',
                        glyph: 0xf130,
                        cls: 'custom-promo-toolbar-button',
                        itemId: 'btn_promoActivity_step2',
                        text: '<b>' + l10n.ns('tpm', 'promoStap').value('promoActivityStep2') + '</b>',
                        tooltip: l10n.ns('tpm', 'promoStap').value('promoActivityStep2'),
                        height: 80,
                        width: 300,
                        margin: '5 5 0 5',
                        isComplete: false
                    }],
                }, {
                    xtype: 'custombottomtoolbar',
                    dock: 'bottom',
                    margin: '5 0 0 0'
                }],

                items: [{
                    xtype: 'panel',
                    itemId: 'promoActivityPanel',
                    stopRefreshScroll: true,
                    name: 'promoActivityContainer',
                    stopRefreshScroll: true,
                    autoScroll: true,
                    cls: 'scrollpanel',
                    bodyStyle: { "background-color": "#ECEFF1" },
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                        pack: 'start',
                    },
                    items: [{
                        xtype: 'promoactivity'
                    }],

                    _refreshScroll: this.refreshScroll,

                    listeners: {
                        afterrender: function (component, eOpts) {
                            $(component.getTargetEl().dom).on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
                                var panel = Ext.ComponentQuery.query('#promoActivityPanel')[0].up();

                                var btnStep1 = panel.down('#btn_promoActivity_step1');
                                var btnStep2 = panel.down('#btn_promoActivity_step2');

                                var formStep1 = panel.down('#promoActivity_step1');
                                var formStep2 = panel.down('#promoActivity_step2');

                                if (formStep2.needToSetHeight && isAtTop) {
                                    formStep2.setHeight(panel.getHeight() - 20);
                                    formStep2.needToSetHeight = false;
                                } else {
                                    component._refreshScroll(component);
                                }

                                var h1 = formStep1.getHeight();
                                var h1_2 = h1 + formStep2.getHeight();

                                var _deltaY = scrollPositionY + 100;

                                // Step 1 
                                if (_deltaY <= h1) {
                                    btnStep1.addClass('selected');
                                    btnStep2.removeCls('selected');

                                    formStep1.header.addClass('promo-header-item-active');
                                    formStep2.header.removeCls('promo-header-item-active');

                                    // Step 2 
                                } else if (_deltaY > h1 && _deltaY <= h1_2) {
                                    btnStep1.removeCls('selected');
                                    btnStep2.addClass('selected');

                                    formStep1.header.removeCls('promo-header-item-active');
                                    formStep2.header.addClass('promo-header-item-active');
                                } else {
                                    console.log('undefined step');
                                }
                            });

                            var mainTab = component.up('promoeditorcustom').down('button[itemId=btn_promoActivity]');
                            var stepButtons = component.up('panel[name=promoActivity]').down('custompromotoolbar');
                            checkMainTab(stepButtons, mainTab);
                        }
                    }
                }]
            }, {
                    xtype: 'promosummary'
            }, {
                // approvalhistory
                items: [{
                    xtype: 'approvalhistory',
                }]
            }]
        }]
    }],

    buttons: [{
        xtype: 'button',
        itemId: 'btn_publish',
        glyph: 0xf12c,
        text: l10n.ns('tpm', 'customtoptoolbar').value('publish'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
        statuses: ['Draft'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
    },
    {
        xtype: 'button',
        itemId: 'btn_splitpublish',
        text: l10n.ns('tpm', 'customtoptoolbar').value('splitpublish'),
        cls: 'promo-split-button',
        hidden: true,
        isPromoAction: true,
        roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
        statuses: ['Draft'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#f7e401" },
        disabled: true
    },
    // Вернуть из Draft Publish в Publish
    {
        xtype: 'button',
        itemId: 'btn_undoPublish',
        glyph: 0xf54d,
        text: l10n.ns('tpm', 'customtoptoolbar').value('undoPublish'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
        statuses: ['DraftPublished'],
        statusId: null,
        statusName: null,
        statusSystemName: null
    },
    // Отправить промо на согласование
    {
        xtype: 'button',
        itemId: 'btn_sendForApproval',
        glyph: 0xf12c,
        text: l10n.ns('tpm', 'customtoptoolbar').value('sendForApproval'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
        statuses: ['DraftPublished'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
		},
	// Вернуть промо в DraftPublished
    {
        xtype: 'button',
        itemId: 'btn_backToDraftPublished',
		glyph: 0xf54d,
		text: l10n.ns('tpm', 'customtoptoolbar').value('backToDraftPublished'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
        roles: ['KeyAccountManager', 'FunctionalExpert', 'Administrator'],
        statuses: ['OnApproval'],
        statusId: null,
        statusName: null,
		statusSystemName: null,
		style: { "background-color": "#66BB6A" }
    },
    // Отклонить промо
    {
        xtype: 'button',
        itemId: 'btn_reject',
        glyph: 0xf156,
        text: l10n.ns('tpm', 'customtoptoolbar').value('reject'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
        roles: ['CMManager', 'CustomerMarketing', 'DemandPlanning', 'DemandFinance'],
        statuses: ['OnApproval'],
        statusId: null,
        statusName: null,
        statusSystemName: null
    },
    // Согласовать промо
    {
        xtype: 'button',
        itemId: 'btn_approve',
        glyph: 0xf12c,
        text: l10n.ns('tpm', 'customtoptoolbar').value('approve'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['CMManager', 'DemandPlanning', 'DemandFinance'],
        statuses: ['OnApproval'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
    },
    // Отменить промо
    {
        xtype: 'button',
        itemId: 'btn_cancel',
        glyph: 0xf739,
        text: l10n.ns('tpm', 'customtoptoolbar').value('cancel'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'KeyAccountManager', 'FunctionalExpert'],
        statuses: ['Approved', 'Planned', 'Started'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#ffb74d" }
    },
    // Спланировать промо
    {
        xtype: 'button',
        itemId: 'btn_plan',
        glyph: 0xf12c,
        text: l10n.ns('tpm', 'customtoptoolbar').value('plan'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'KeyAccountManager', 'FunctionalExpert'],
        statuses: ['Approved'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
    },
    // Закрыть промо
    {
        xtype: 'button',
        itemId: 'btn_close',
        glyph: 0xf12c,
        text: l10n.ns('tpm', 'customtoptoolbar').value('close'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'KeyAccountManager', 'FunctionalExpert'],
        statuses: ['Finished'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
    }, {
        xtype: 'button',
        itemId: 'btn_backToFinished',
        glyph: 0xf54d,
        text: l10n.ns('tpm', 'customtoptoolbar').value('backtofinished'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
		roles: ['Administrator', 'FunctionalExpert'],
        statuses: ['Closed'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
    }, {
        xtype: 'button',
        itemId: 'btn_changeStatus',
        glyph: 0xf4e1,
        text: l10n.ns('tpm', 'customtoptoolbar').value('changestatus'),
        cls: 'promo-action-button',
        hidden: true,
        isPromoAction: true,
        roles: ['SupportAdministrator'],
        statuses: ['Draft', 'DraftPublished', 'OnApproval', 'Approved', 'Planned', 'Started', 'Finished', 'Closed', 'Cancelled'],
        statusId: null,
        statusName: null,
        statusSystemName: null,
        style: { "background-color": "#66BB6A" }
    }, {
        xtype: 'tbspacer',
        flex: 10
        },
        {
        text: l10n.ns('tpm', 'customtoptoolbar').value('recalculate'),
        itemId: 'btn_recalculatePromo',
        style: { "background-color": "#ffb74d" },
        hidden: true
        },
        {
            text: l10n.ns('tpm', 'customtoptoolbar').value('resetPromo'),
            itemId: 'btn_resetPromo',
            style: { "background-color": "#ffb74d" },
            hidden: true
        },{
        text: l10n.ns('tpm', 'buttons').value('close'),
        itemId: 'closePromo',
        style: { "background-color": "#3F6895" },
        hidden: true
    }, {
        text: l10n.ns('tpm', 'buttons').value('cancel'),
        itemId: 'cancelPromo',
        style: { "background-color": "#3F6895" }
    }, {
        text: l10n.ns('tpm', 'buttons').value('edit'),
        itemId: 'changePromo',
        style: { "background-color": "#26A69A" },
        roles: ['Administrator', 'CMManager', 'CustomerMarketing', 'FunctionalExpert', 'KeyAccountManager', 'DemandPlanning', 'DemandFinance'],
        hidden: true
    }, {
        text: l10n.ns('tpm', 'promoButtons').value('ok'),
        itemId: 'savePromo',
        style: { "background-color": "#66BB6A" }
    }, {
        text: l10n.ns('tpm', 'promoButtons').value('saveAndClose'),
        itemId: 'saveAndClosePromo',
        style: { "background-color": "#26A69A" }
    }],

    blockedLoading: false,
    setLoading: function (load, targetEl, blockLoad) {
        var me = this,
            config = {
                target: me
            };
        if (!me.blockedLoading && blockLoad !== false) {
            if (me.rendered) {
                Ext.destroy(me.loadMask);
                me.loadMask = null;

                if (load !== false && !me.collapsed) {
                    if (Ext.isObject(load)) {
                        Ext.apply(config, load);
                    } else if (Ext.isString(load)) {
                        config.msg = load;
                    }

                    if (targetEl) {
                        Ext.applyIf(config, {
                            useTargetEl: true
                        });
                    }
                    me.loadMask = new Ext.LoadMask(config);
                    me.loadMask.show();
                }
            }
            return me.loadMask;
        }
        if (blockLoad !== undefined) {
            me.blockedLoading = blockLoad;
        }
    }
});