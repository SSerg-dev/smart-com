var refreshScroll = function refreshScroll(panel) {
    var targetEl = panel.getTargetEl();

    if (!targetEl) {
        return;
    }

    var domEl = $(targetEl.dom);

    if (!panel.hasScroll) {
        domEl.jScrollPane({
            verticalGutter: 35
        });

        domEl.bind(
            'jsp-initialised',
            function (event, isScrollable) {
                var jspData = domEl.data('jsp');

                if (jspData.getIsScrollableV()) {
                    domEl.addClass('has-vertical-scroll');
                } else {
                    domEl.removeClass('has-vertical-scroll');
                }

                if (jspData.getIsScrollableH()) {
                    domEl.addClass('has-horizontal-scroll');
                } else {
                    domEl.removeClass('has-horizontal-scroll');
                }
            }
        );

        domEl.bind(
            'jsp-scroll-x',
            function (event, scrollPositionX, isAtLeft, isAtRight) {
                if (!Ext.isNumber(scrollPositionX)) return;

                if (isExplorer()) {
                    var event = document.createEvent('WheelEvent', { "deltax": scrollPositionX, "deltamode": 0 });
                    event.initEvent('scroll', true, true)
                    targetEl.dom.dispatchEvent(event);
                } else {
                    targetEl.dom.dispatchEvent(new WheelEvent('scroll', {
                        deltaX: scrollPositionX,
                        deltaMode: 0
                    }));
                }
            }
        );

        domEl.bind(
            'jsp-scroll-y',
            function (event, scrollPositionY, isAtTop, isAtBottom) {
                var menuItems = panel.query('menuitem[hidden=false]');
                var shownMenuItems = [];
                for (var i = 0; i < menuItems.length; i++) {
                    if (menuItems[i].getTargetEl() != null) {
                        shownMenuItems.push(menuItems[i]);
                    }
                }

                if (isExplorer()) {
                    var event = document.createEvent('WheelEvent', { "deltay": 4, "deltamode": 0 });
                    event.initEvent('scroll', true, true)
                    targetEl.dom.dispatchEvent(event);
                }
                else {
                    for (var i = 0; i < shownMenuItems.length; i++) {
                        var mainPanel = shownMenuItems[i].up('combineddirectorypanel');
                        if (mainPanel != null) {
                            var headerHeight = mainPanel.down('header').getHeight();
                            var currentParentMenu = null;
                            var menuPanel = shownMenuItems[i].parentMenu;
                            if (menuPanel == currentParentMenu) continue;
                            if (menuPanel != null) {
                                menuPanel.setY(mainPanel.getY() + headerHeight);
                                menuPanel.el.setStyle('zIndex', 1);
                                menuPanel.el.shadow.zIndex = 1;
                                currentParentMenu = menuPanel;
                            }
                        }
                    }
                    if (Ext.isNumber(scrollPositionY)) {
                        targetEl.dom.dispatchEvent(new WheelEvent('scroll', {
                            deltaY: scrollPositionY,
                            deltaMode: 0
                        }));
                    }
                }
            }
        );

        panel.hasScroll = true;
    } else if (panel.hasScroll) {
        domEl.data('jsp').reinitialise();
    }
};

function checkMainTab(stepButtons, mainTab) {
    var status = true;

    for (i = 0; i < stepButtons.items.items.length; i++) {
        if (!stepButtons.items.items[i].isComplete) {
            status = false;
            break;
        }
    }

    var el = mainTab.getEl();

    if (el) {
        var stepCheck = el.dom.getElementsByClassName('custom-promo-main-step-check')[0];

        if (status) {
            stepCheck.innerText = '';
            stepCheck.style.color = '#3F6895';
        } else {
            stepCheck.innerText = '';
            stepCheck.style.color = '#ADBAC0';
        }
    }
};

var budgetsChangeListener = function (field, newValue, oldValue) {
    var container = field.up('container[buttonId]');
    var fields = Ext.ComponentQuery.query('#' + container.id + ' [name=planFieldSetBudgets] numberfield');
    var status = true;

    for (i = 0; i < fields.length; i++) {
        if ((fields[i].getValue() === null || !fields[i].isValid()) && fields[i].isChecked === true) {
            status = false;
            break;
        }
    }

    var panel = field.up('promoeditorcustom');
    var currentButton = panel.down('button[itemId=' + container.buttonId + ']');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = panel.down('button[itemId=btn_promoBudgets]');
    var stepButtons = currentButton.up('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};

var invoiceAndPricesChangeListener = function (field, newValue, oldValue) {
    var status = true;

    var panel = field.up('panel[name=promoActivity_step1]');
    var fields1 = panel.down('fieldset[name=instoreAssumptionFieldset]').items.items;
    var fields2 = panel.down('fieldset[name=invoiceAndPricesFieldset]').items.items;

    for (i = 0; i < fields1.length; i++) {
        if ((fields1[i].getValue() === null || !fields1[i].isValid()) && fields1[i].isChecked === true) {
            status = false;
            break;
        }
    }

    for (i = 0; i < fields2.length; i++) {
        if ((fields2[i].getValue() === null || !fields2[i].isValid()) && fields2[i].isChecked === true) {
            status = false;
            break;
        }
    }

    var promoeditorcustom = field.up('promoeditorcustom');
    var currentButton = promoeditorcustom.down('button[itemId=btn_promoActivity_step1]');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = promoeditorcustom.down('button[itemId=btn_promoActivity]');
    var stepButtons = currentButton.up('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};

var activityChangeListener = function (field, newValue, oldValue) {
    var status = true;

    var panel = field.up('panel[name=promoActivity_step2]');
    var fields1 = panel.down('fieldset[name=activity]').items.items;

    for (i = 0; i < fields1.length; i++) {
        if ((fields1[i].getValue() === null || !fields1[i].isValid()) && fields1[i].isChecked === true) {
            status = false;
            break;
        }
    }

    var promoeditorcustom = field.up('promoeditorcustom');
    var currentButton = promoeditorcustom.down('button[itemId=btn_promoActivity_step2]');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = promoeditorcustom.down('button[itemId=btn_promoActivity]');
    var stepButtons = currentButton.up('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};


// ToDo: Удалить
var calculationChangeListener = function (field, newValue, oldValue) {
    var container = field.up('container[buttonId]');
    var fields = Ext.ComponentQuery.query('#' + container.id + ' numberfield');
    var status = true;

    for (i = 0; i < fields.length; i++) {
        if (fields[i].getValue() === null || !fields[i].isValid()) {
            status = false;
            break;
        }
    }

    var panel = field.up('promoeditorcustom');
    var currentButton = panel.down('button[itemId=' + container.buttonId + ']');

    if (status) {
        currentButton.isComplete = status;
        currentButton.removeCls('notcompleted');
        currentButton.setGlyph(0xf133);
    } else {
        currentButton.isComplete = status;
        currentButton.addCls('notcompleted');
        currentButton.setGlyph(0xf130);
    }

    var mainTab = panel.down('button[itemId=btn_calculations]');
    var stepButtons = panel.down('panel[itemId=calculations]').down('custompromotoolbar');

    checkMainTab(stepButtons, mainTab);
};

// ToDo: Удалить
var calculationChangePostPromoEffectListener = function (field, newValue, oldValue) {
    postPromoEffectFields = getPostPromoEffectFields(field.up('promocalculation'));

    postPromoEffectFields.PlanPostPromoEffect.setValue(
        postPromoEffectFields.PlanPostPromoEffectW1.getValue() + postPromoEffectFields.PlanPostPromoEffectW2.getValue()
    );
};

var getPostPromoEffectFields = function (widgetName) {
    return {
        PlanPostPromoEffectW1: widgetName.down('numberfield[name=PlanPostPromoEffectW1]'),
        PlanPostPromoEffectW2: widgetName.down('numberfield[name=PlanPostPromoEffectW2]'),
        PlanPostPromoEffect: widgetName.down('numberfield[name=PlanPostPromoEffect]'),
        FactPostPromoEffectW1: widgetName.down('numberfield[name=FactPostPromoEffectW1]'),
        FactPostPromoEffectW2: widgetName.down('numberfield[name=FactPostPromoEffectW2]'),
        FactPostPromoEffect: widgetName.down('numberfield[name=FactPostPromoEffect]'),
    }
};

function setHighlightMargins(slider, value) {
    if (slider.highlightEl) {
        var range = (slider.maxValue || 100) - (slider.minValue || 0);

        var position1 = Math.abs((slider.minValue || 0) - slider.minValue);
        var position2 = range - Math.abs((slider.minValue || 0) - value);

        var percent1 = position1 > 0 ? 100.0 / range * position1 : 0;
        var percent2 = position2 > 0 ? 100.0 / range * position2 : 0;

        slider.highlightEl.setStyle({
            marginLeft: percent1 + '%',
            marginRight: percent2 + '%'
        });
    }
};

function renderWithDelimiter(value, delimFrom, delimTo) {
    return value.split(delimFrom).join('<span style="font-size: 13px; font-family: MaterialDesignIcons; color: #A7AFB7">' + delimTo + '</span>');
}

function dateDiff(str1, str2) {
    var diff = Date.parse(str2) - Date.parse(str1);
    return isNaN(diff) ? NaN : {
        diff: diff,
        ms: Math.ceil(diff % 1000),
        s: Math.ceil(diff / 1000 % 60),
        m: Math.ceil(diff / 60000 % 60),
        h: Math.ceil(diff / 3600000 % 24),
        d: Math.ceil(diff / 86400000)
    };
}

//Обход параметров ядра для стилей

var bbStyleToOverrryde = {
    width: 60,
    minWidth: 60,
    maxWidth: 250,
    cls: 'custom-scheduler-toolbar',
    defaults: {
        ui: 'gray-button-toolbar',
        //padding: 6, //TODO: временно
        textAlign: 'center'
    },
    listeners: {
        beforerender: function (tb) {
            for (var i = 0; i < tb.items.items.length; i++) {
                var item = tb.items.items[i]
                if (item.itemId == 'table' || item.itemId == 'detail') {
                    item.setVisible(false);
                }
            }
        }
    }
};
Ext.override(App.view.core.toolbar.ReadonlyDeletedDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.ReadonlyDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.HardDeleteDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.StandardDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.ReadonlyWithoutFilterDirectoryToolbar, bbStyleToOverrryde);
Ext.override(App.view.core.toolbar.AddOnlyDirectoryToolbar, bbStyleToOverrryde);

Ext.override(App.view.core.filter.ExtendedFilterWindow, {
    cls: 'scrollable custom-scheduler-toolbar',
    listeners: {
        beforerender: function (win) {
            // Изменение ширины тулбара
            var tb = win.down('#filtertoolbar');
            if (tb) {
                tb.width = 60;
                tb.minWidth = 60;
                tb.cls = 'custom-scheduler-toolbar';
                tb.defaults = {
                    ui: 'gray-button-toolbar',
                    //padding: 6, //TODO: временно,
                    textAlign: 'center'
                }
                for (var i = 0; i < tb.items.items.length; i++) {
                    var item = tb.items.items[i]
                    item.padding = null;
                    item.textAlign = 'center';
                }
            }
        }
    }
});

Ext.override(App.view.core.toolbar.ReportsFilterToolbar, {
    width: 60,
    minWidth: 60,
    maxWidth: 250,
    cls: 'custom-scheduler-toolbar',
    defaults: {
        ui: 'gray-button-toolbar',
        //padding: 6, //TODO: временно
        textAlign: 'center'
    },
    items: [{
        xtype: 'widthexpandbutton',
        ui: 'fill-gray-button-toolbar',
        text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
        glyph: 0xf13d,
        glyph1: 0xf13e,
        target: function () {
            return this.up('toolbar');
        }
    }]
});

// Заменяет символ иерархии на '>' в treefsearchfield
Ext.override(App.view.tpm.nonenego.TreeFilterableSearchField, {
    onChange: function (searchfield) {
        if (searchfield) {
            var me = this;
            Ext.each(searchfield.values, function (val) {
                if (val.value.indexOf('  ') >= 0)
                    me.setValue(val.value.replace(//g, '>'));
            });
        }
    },
});

// Заменяет символ иерархии на '>' в textfield
Ext.override(Ext.form.field.Text, {
    onChange: function (value) {
        // проверяем по xtype потому, что datefield тоже срабатывает
        if (this.xtype == 'textfield' && value && value.indexOf('  ') >= 0) {
            this.setValue(value.replace(//g, '>'));
        }
    },
});

var calculationUpliftCheckbox = function (newValue) {
    var planUplift = Ext.ComponentQuery.query('[name=PlanPromoUpliftPercent]')[0];

    if (newValue.value === true) {
        planUplift.setReadOnly(false);
        planUplift.removeCls('readOnlyField');
    } else {
        planUplift.setReadOnly(true);
        planUplift.addCls('readOnlyField');
    }
}

// Дополнитеьная валидация для правильного отображения измеенённых полей
Ext.override(App.view.core.common.SearchComboBox, {
    afterSetValue: function (record) {
        if (this.needUpdateMappings) {
            this.updateMappingValues(record);
        }
        //Валидация всей формы после изменения
        var f = this.up('form');
        if (f && f.getForm) {
            var form = f.getForm();
            form.isValid();
        }
    }
});


// Чтобы делать отображение в всплывающих окнах
var editorChangeFunction = function (button) {
    var grid = this.getGridByButton(button),
        selModel = grid.getSelectionModel();

    if (!grid.editorModel || grid.editorModel.name != 'ToChangeEditorDetailWindowModel') {
        grid.editorModel = Ext.create('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
            grid: grid
        });
    }
    if (selModel.hasSelection()) {
        grid.editorModel.startDetailRecord(selModel.getSelection()[0]);
    } else {
        console.log('No selection');
    }
};

var updateEditorChangeFunction = function (button) {
    var grid = this.getGridByButton(button),
        selModel = grid.getSelectionModel();
    if (!grid.editorModel || grid.editorModel.name != 'ToChangeEditorDetailWindowModel') {
        grid.editorModel = Ext.create('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
            grid: grid
        });
    }
    if (selModel.hasSelection()) {
        grid.editorModel.startEditRecord(selModel.getSelection()[0]);
    } else {
        console.log('No selection');
    }
};

Ext.override(App.controller.core.role.Role, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.role.HistoricalRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.role.DeletedRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associateduser.user.AdUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.aduser.AssociatedAdUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.userrole.AssociatedUserRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.dbuser.DeletedAssociatedDbUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associateduser.dbuser.HistoricalAssociatedDbUser, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associatedconstraint.constraint.AssociatedConstraint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedconstraint.userrole.AssociatedUserRoleMain, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.setting.Setting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.setting.HistoricalSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.loophandler.AdminLoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.loophandler.HistoricalLoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.loophandler.LoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.loophandler.UserLoopHandler, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

Ext.override(App.controller.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

// Настройка почтовых уведомлений (верхний грид)
Ext.override(App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.DeletedAssociatedMailNotificationSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });
Ext.override(App.controller.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting, { switchToDetailForm: editorChangeFunction, onUpdateButtonClick: updateEditorChangeFunction });

// Настройка почтовых уведомлений (нижний грид)
Ext.override(App.controller.core.associatedmailnotificationsetting.recipient.AssociatedRecipient, { switchToDetailForm: editorChangeFunction });
Ext.override(App.controller.core.associatedmailnotificationsetting.recipient.HistoricalAssociatedRecipient, { switchToDetailForm: editorChangeFunction });


// custom Vtype
Ext.apply(Ext.form.field.VTypes, {
    eanNum: function (v) {
        return /^\d{0,13}$/.test(v);
    },
    eanNumText: 'Must be a numeric EAN',
    eanNumMask: /\d{0,13}/i
});

//Правка для IE при смене фокуса в гриде
Ext.override(Ext.view.Table, {
    focusRow: function (rowIdx) {
        var me = this,
        row,
        gridCollapsed = me.ownerCt && me.ownerCt.collapsed,
        record;

        if (me.isVisible(true) && !gridCollapsed && (row = me.getNode(rowIdx, true)) && me.el) {
            record = me.getRecord(row);
            rowIdx = me.indexInStore(row);

            me.selModel.setLastFocused(record);
            row.focus();
            me.focusedRow = row;
            me.fireEvent('rowfocus', rowIdx, true, Ext.isIE);

        }
    },
});
