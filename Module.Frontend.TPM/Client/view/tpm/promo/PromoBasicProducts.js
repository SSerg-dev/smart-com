Ext.define('App.view.tpm.promo.PromoBasicProducts', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promobasicproducts',

    // запись о выбранных продуктах (см. модель Promo.cs C#)
    promoProductRecord: null,
    // ObjectID изначально выбранных (чекнутых) продуктов
    choosenProductObjectIds: [],
    // аббревиатура бренда
    brandAbbreviation: '',
    // аббревиатура технологии
    technologyAbbreviation: '',
    // путь к выбранному узлу
    fullPath: '',
    // блокировка дат для промо в некоторых статусах
    treesChangingBlockDate: false,
    // нужно ли Disable'ить кнопки Subranges
    disableBtns: false,

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [{
            xtype: 'custompromopanel',
            name: 'chooseProduct',
            minWidth: 245,
            height: 208,
            flex: 1,
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'center'
            },
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'PromoBasicProducts').value('ChooseProducts'),
                padding: '1 4 9 4',
                height: 160,
                items: [{
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'container',
                        padding: '0 5 0 7',
                        items: [{
                            xtype: 'button',
                            itemId: 'choosePromoProductsBtn',
                            glyph: 0xf968,
                            scale: 'large',
                            height: 98,
                            width: 110,
                            text: '<b>' + l10n.ns('tpm', 'PromoBasicProducts').value('ChooseProduct') + '<br/>...</b>',
                            iconAlign: 'top',
                            cls: 'custom-event-button promobasic-choose-btn',
                            disabledCls: 'promobasic-choose-btn-disabled',
                        }]
                    }, {
                        xtype: 'container',
                        flex: 1,
                        padding: '0 5 0 7',
                        items: [{
                            xtype: 'singlelinedisplayfield',
                            name: 'PromoProductBrand',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'PromoBasicProducts').value('Brand'),
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'PromoProductTechnology',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'PromoBasicProducts').value('Technology')
                        }]
                    }]
                }]
            }, {
                xtype: 'container',
                height: 33,
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'top',
                    pack: 'center'
                },
                items: [{
                    xtype: 'tbspacer',
                    flex: 1
                }, {
                    xtype: 'button',
                    width: 111,
                    padding: '3 7 3 10',
                    cls: 'hierarchyButton hierarchyButtonList',
                    itemId: 'promoBasicProducts_ProductList',
                    text: l10n.ns('tpm', 'button').value('ProductList'),
                    tooltip: l10n.ns('tpm', 'button').value('ProductList'),
                    iconCls: 'icon-list-png'
                }]
            }]
        }, {
            xtype: 'splitter',
            itemId: 'splitter_chooseProduct',
            cls: 'custom-promo-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter#splitter_chooseProduct')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'custompromopanel',
            minWidth: 245,
            height: 208,
            flex: 1,
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'center'
            },
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'PromoBasicProducts').value('SelectedSubranges'),
                padding: '0 0 0 10',
                height: 160,
                //itemId: 'choosenSubrangesPanel',
                layout: 'fit',
                items: [{
                    xtype: 'container',
                    itemId: 'choosenSubrangesPanel',
            autoScroll: true,
            flex: 1,
            height: '100%',
            width: '100%',
            cls: 'scrollpanel hScrollPanel',
            layout: {
                type: 'hbox',
                align: 'top',
            },
            padding: '0 0 2 1',
            defaults: {
                margin: '0 10 10 0',
            },
            items: []
        }]
            },
            {
                xtype: 'container',
                height: 33,
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'top',
                    pack: 'center'
                },
                items: [{
                    xtype: 'tbspacer',
                    flex: 1
                }, {
                    xtype: 'button',
                    width: 140,
                    cls: 'hierarchyButton hierarchyButtonList',
                    itemId: 'promoBasicProducts_FilteredList',
                    text: l10n.ns('tpm', 'button').value('FilteredProductList'),
                    tooltip: l10n.ns('tpm', 'button').value('FilteredProductList'),
                    glyph: 0xf232,
                }]
            }]
        }]
    }],

    // заполнение формы при загрузке промо
    fillFormJson: function (promoBasicProductJSON, treesChangingBlockDate) {
        this.treesChangingBlockDate = treesChangingBlockDate;
        this.promoProductRecord = promoBasicProductJSON ? JSON.parse(promoBasicProductJSON) : null;
        this.brandAbbreviation = this.promoProductRecord.BrandAbbreviation;
        this.technologyAbbreviation = this.promoProductRecord.TechnologyAbbreviation;
        this.fillMainInfo();
    },

    fillForm: function (nodesProductTree) {
        var me = this;

        if (nodesProductTree.length > 0) {
            me.promoProductRecord = {
                Brand: null,
                Technology: null,
                LogoFileName: '',
                ProductsChoosen: []
            };

            nodesProductTree.forEach(function (node) {
                me.promoProductRecord.ProductsChoosen.push({
                    ObjectId: node.get('ObjectId'),
                    Name: node.get('Name'),
                    Type: node.get('Type'),
                    LogoFileName: node.get('LogoFileName'),
                    FullPathName: node.get('FullPathName'),
                    Filter: node.get('Filter'),
                });
            });

            var current = nodesProductTree[0];
            while (current && current.data.root !== true) {
                if (current.data.Type.indexOf('Brand') >= 0) {
                    me.promoProductRecord.Brand = current.data.Name;
                    me.brandAbbreviation = current.data.Abbreviation;

                    if (!me.promoProductRecord.LogoFileName || me.promoProductRecord.LogoFileName.length == 0)
                        me.promoProductRecord.LogoFileName = current.data.LogoFileName;
                }
                else if (current.data.Type.indexOf('Technology') >= 0) {
                    me.promoProductRecord.Technology = current.data.Name;
                    me.technologyAbbreviation = current.data.Abbreviation;
                    me.promoProductRecord.LogoFileName = current.data.LogoFileName;
                }

                current = current.parentNode;
            }
        }
        else {
            this.promoProductRecord = null;
            this.fullPath = '';
        }

        this.fillMainInfo();
    },

    fillMainInfo: function () {
        var chooseBtn = this.down('#choosePromoProductsBtn');

        if (this.promoProductRecord) {
            // подпись для кнопки выбора
            var nameForBtn = this.promoProductRecord.Brand ? this.promoProductRecord.Brand : this.promoProductRecord.Technology;
            var iconSrc = this.promoProductRecord.LogoFileName ? '/odata/ProductTrees/DownloadLogoFile?fileName=' + encodeURIComponent(this.promoProductRecord.LogoFileName) : '/bundles/style/images/swith-glyph-gray.png';

            chooseBtn.setText('<b>' + nameForBtn + '<br/>...</b>');
            chooseBtn.setGlyph();
            chooseBtn.setIcon(iconSrc);
            chooseBtn.setIconCls('promoClientChooseBtnIcon');

            this.down('[name=PromoProductBrand]').setValue(this.promoProductRecord.Brand);
            this.down('[name=PromoProductTechnology]').setValue(this.promoProductRecord.Technology);
        } else {
            chooseBtn.setText('<b>' + l10n.ns('tpm', 'PromoBasicProducts').value('ChooseProduct') + '<br/>...</b>');
            chooseBtn.setIcon();
            chooseBtn.setIconCls('x-btn-glyph materialDesignIcons');
            chooseBtn.setGlyph(0xf968);

            this.down('[name=PromoProductBrand]').setValue(null);
            this.down('[name=PromoProductTechnology]').setValue(null);
        }

        // для обновления отрисовки кнопки
        chooseBtn.fireEvent('resize', chooseBtn);
        // заполняем панель выбранных subranges
        this.fillSubrangePanel();
    },

    // заполнить панель выбранных subranges
    fillSubrangePanel: function () {
        var me = this;
        var subrangeBtns = [];
        var subrangePanel = me.down('#choosenSubrangesPanel');

        me.choosenProductObjectIds = [];
        subrangePanel.removeAll();

        // если запись пуста, то ничего не добавляем
        if (me.promoProductRecord) {
            var choosenNodes = me.promoProductRecord.ProductsChoosen;

            // устанавливаем путь к узлу
            me.fullPath = choosenNodes[0].FullPathName;
            // если выбрано более одного, то последю часть заменяем на "..."
            if (choosenNodes.length > 1) {
                var indexOfLastArrow = me.fullPath.lastIndexOf('>');
                me.fullPath = me.fullPath.substring(0, indexOfLastArrow + 1) + ' ...';
            }

            choosenNodes.forEach(function (item, index) {
                me.choosenProductObjectIds.push(item.ObjectId);

                // парсим фильтр из Json
                item.Filter = item.Filter && item.Filter.length > 0 ? JSON.parse(item.Filter) : null;

                // фильтруем только с типом subrange (можно же выбрать просто технологию например)
                if (item.Type.toLowerCase().indexOf('subrange') >= 0) {
                    var iconSrc = item.LogoFileName ? '/odata/ProductTrees/DownloadLogoFile?fileName=' + encodeURIComponent(item.LogoFileName) : '/bundles/style/images/swith-glyph-gray.png';

                    subrangeBtns.push({
                        xtype: 'button',
                        scale: 'large',
                        height: 98,
                        width: 110,
                        text: '<b>' + item.Name + '<br/></b>',
                        iconAlign: 'top',
                        icon: iconSrc,
                        iconCls: 'promoClientChooseBtnIcon',
                        cls: 'custom-event-button promobasic-choose-btn cursor-pointer',
                        disabled: true,
                        disabledCls: '',
                        style: 'opacity: 1.0 !important; cursor: default',
                    });
                }
            });
        }

        subrangePanel.add(subrangeBtns);
    },

    // вызвать форму выбора продуктов
    chooseProducts: function (callBackChooseFnc) {
        var productObjectIds = this.choosenProductObjectIds; // текущие выбранные узлы
        var blockDate = this.treesChangingBlockDate; // блокировка по датам

        var choosePromoProductWind = Ext.create('App.view.tpm.promo.PromoProductChooseWindow', {
            choosenProductObjectIds: productObjectIds,
            treesChangingBlockDate: blockDate,
            callBackChooseFnc: callBackChooseFnc
        });

        choosePromoProductWind.show();
    },

    // возвращает общий фильтр для выбранных Subranges
    getFilterForSubranges: function () {
        var filter = null;

        if (this.promoProductRecord) {
            var choosenNodes = this.promoProductRecord.ProductsChoosen;
            filter = {
                operator: 'or',
                rules: []
            };

            for (var i = 0; i < choosenNodes.length; i++) {
                // если нет фильтра хотябы у одного subrange, то общий фильтр пуст
                if (choosenNodes[i].Filter)
                    filter.rules.push(this.parseFilter(choosenNodes[i].Filter));
                else {
                    filter = null;
                    break;
                }
            }
        }

        return filter;
    },

    // парсим сырой фильтр в пригодный для Store
    parseFilter: function (filterRaw) {
        var customTextFilterModel = Ext.create('App.model.tpm.filter.CustomTextFilterModel'); // с помощью него делаем фильтр для store
        var filter = customTextFilterModel.deserializeFilter(filterRaw); // формируем конечный фильтр

        var jsonObj = {
            rules: [],
            operator: filter.operation
        };

        if (!filter.operator)
            return null;

        jsonObj.operator = filter.operator;
        var ruleContent = filter.rules;

        for (var i = 0; i < ruleContent.length; i++) {
            var currentRule = ruleContent[i];

            if (currentRule.operation) {
                if (currentRule.operation === 'In') {
                    var jsonInObj = {
                        rules: [],
                        operator: 'or'
                    };

                    for (var j = 0; j < currentRule.value.values.length; j++) {
                        var rule = {
                            operation: "Equals",
                            property: currentRule.property,
                            value: currentRule.value.values[j],
                        };

                        jsonInObj.rules.push(rule);
                    }

                    jsonObj.rules.push(jsonInObj);

                }
                else if (currentRule.operation === 'IsNull') {
                    var jsonIsNullObj = {
                        rules: [],
                        operator: 'or'
                    };

                    jsonIsNullObj.rules.push({
                        operation: "Equals",
                        property: currentRule.property,
                        value: null,
                    });

                    jsonIsNullObj.rules.push({
                        operation: "Equals",
                        property: currentRule.property,
                        value: '',
                    });

                    jsonObj.rules.push(jsonIsNullObj);
                }
                else if (currentRule.operation === 'NotNull') {
                    var jsonNotNullObj = {
                        rules: [],
                        operator: 'and'
                    };

                    jsonNotNullObj.rules.push({
                        operation: "NotEqual",
                        property: currentRule.property,
                        value: null,
                    });

                    jsonNotNullObj.rules.push({
                        operation: "NotEqual",
                        property: currentRule.property,
                        value: '',
                    });

                    jsonObj.rules.push(jsonNotNullObj);
                }
                else {
                    jsonObj.rules.push({
                        operation: currentRule.operation,
                        property: currentRule.property,
                        value: currentRule.value ? currentRule.value : '',
                    });
                }
            }
            else if (currentRule.operator) {
                jsonObj.rules.push(this.parseFilter(currentRule))
            }
        }

        if (jsonObj.rules.length == 0)
            return null;

        return jsonObj;
    },

    // переключение вида кнопок на редактирование/просмотр
    setDisabledBtns: function (disable) {
        var subrangesBtns = this.down('#choosenSubrangesPanel').items.items;
        this.disableBtns = disable;

        for (var i = 0; i < subrangesBtns.length; i++) {
            if (disable) {
                subrangesBtns[i].removeCls('cursor-pointer');
            }
            else {
                subrangesBtns[i].addCls('cursor-pointer');
            }
        }

        this.down('#choosePromoProductsBtn').setDisabled(disable);
    }
})