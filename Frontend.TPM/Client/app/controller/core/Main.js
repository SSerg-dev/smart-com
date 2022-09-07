Ext.define('App.controller.core.Main', {
    extend: 'Ext.app.Controller',

    maxMenuHistory: 3, //TODO: перенести

    refs: [{
        ref: 'menuPanel',
        selector: '#drawer'
    }, {
        ref: 'viewContainer',
        selector: '#viewcontainer'
    }, {
        ref: 'menuHistory',
        selector: '#drawer #menuhistory'
    }, {
        ref: 'viewport',
        selector: 'viewport'
    }],

    init: function () {
        this.listen({
            component: {
                'viewport': {
                    beforerender: this.setMenuLogoPath,
                    afterrender: this.onRenderViewport
                },
                'drawer [widget]': {
                    click: this.onOpenViewButtonClick
                },
                'drawer [func]': {
                    click: this.onFunctionButtonClick
                },
                '#viewcontainer': {
                    add: this.onViewAdd,
                    remove: this.onViewRemove,
                    afterrender: this.onRenderViewContainer
                },
                'associateddirectoryview': {
                    afterrender: this.onRenderViewContainer
                },
                '#menucontainer [node]': {
                    click: this.onMenuButtonClick
                },
                'drawer #back': {
                    click: this.onMenuButtonClick
                },
                'window #close': {
                    click: this.onWindowClose
                },
                'window > toolbar[dock=bottom] > #cancel': {
                    click: this.onWindowClose
                },
                '#drawer #menusearchfield': {
                    change: {
                        fn: this.onMenuSearchFieldChange,
                        buffer: 500
                    }
                },
                '#layoutbutton': {
                    click: this.onNotSupportedActionClick
                }
            },
            app: {
                '#menumgr': {
                    menuchange: this.onMenuChange
                }
            }
        });
    },

    onViewAdd: function (viewContainer, item) {
        if (viewContainer.contains(item)) {
            this.highlightCurrentMenuItem();
        }
    },

    onViewRemove: function (viewContainer, item) {
        if (!viewContainer.child()) {
            console.trace('view remove', arguments);
            this.highlightCurrentMenuItem();
            this.getViewport().down('#breadcrumbs').setText('');
        }
    },

    onRenderViewContainer: function (container) {
        container.on({
            scope: container,
            element: 'body',
            scroll: function () {
                if (container.down('associateddirectoryview')) {
                    return;
                }

                var topcurtain = container.up('viewport').down('#topcurtain').getEl(),
                    pane = container.body.down('> .jspContainer > .jspPane'),
                    top = pane ? Math.abs(pane.dom.offsetTop) : 0,
                    newHeight = top > 0 ? 10 : 0;

                if (topcurtain.getHeight() != newHeight) {
                    topcurtain.setHeight(newHeight);
                }
            }
        });
    },

    onNotSupportedActionClick: function () {
        Ext.Msg.show({
            title: l10n.ns('core').value('alertTitle'),
            msg: l10n.ns('core').value('notSupportedMessage'),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO
        });
    },

    onRenderViewport: function (viewport) {
        var menucontainer = viewport.down('#menucontainer');
        MenuMgr.setCurrentMenu(MenuMgr.getCurrentMenu());
    },

    setMenuLogoPath: function () {
        var logo = Ext.ComponentQuery.query('#menulogo')[0];
        var path = location.origin + '/Bundles/style/images/logo.svg';

        var settingStore = Ext.create('App.store.core.settinglocal.SettingLocalStore');
        settingStore.load();

        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value == 1) {
                path = location.origin + '/Bundles/style/images/logo_rs.svg'
            }
        }

        logo.setSrc(path);
    },

    onOpenViewButtonClick: function (button) {
        //Workaround for change user widget if change authentication
        if (button.itemId == 'user' /*&& Ext.isEmpty(button.widget)*/) {
            var authSourceType = App.UserInfo.getAuthSourceType();
            switch (authSourceType) {
                case 'Database':
                    button.widget = 'associateddbusercontainer';
                    break;
                case 'ActiveDirectory':
                    button.widget = 'associatedadusercontainer';
                    break;
                case 'Mixed':
                    alert('Not implemented for Mixed authentication!');
                    break;
                default:
                    alert('Incorrect authentication source type: ' + authSourceType);
                    break;
            }
        }

        var vc = this.getViewContainer(),
            view = vc.getComponent(button.widget),
            menuhistory = this.getMenuHistory();
        //title = Ext.getCmp('maintitle');

        if (!view) {
            view = Ext.widget(button.widget);

            if (view.isXType('associateddirectoryview')) {
                vc.addCls('associated-directory');
            } else {
                vc.removeCls('associated-directory');
            }

            Ext.suspendLayouts();
            vc.removeAll();
            vc.add(view);
            Ext.resumeLayouts(true);

            //title.setText(button.getText());

            // Workaround для решения бага с прокрукой, если контент изначально не влезает во viewcontainer
            vc.doLayout();
        }

        var path = this.buildBreadCrumbsPath(button.node);
        this.getViewport().down('#breadcrumbs').setText(path.join('\\'));

        if (menuhistory && !menuhistory.down('[widget=' + button.widget + ']')) {
            if (menuhistory.items.getCount() >= this.maxMenuHistory) {
                menuhistory.remove(menuhistory.items.last());
            }
            menuhistory.insert(0, button.cloneConfig({
                ui: 'history-menu-button',
                margin: '0 10 5 10',
                scale: 'small'
            }));
            menuhistory.show();
        }
    },

    onWindowClose: function (button) {
        button.up('window').close();
    },

    onMenuButtonClick: function (button) {
        var menuSearch = button.up('drawer').down('#menusearchfield');
        if (button.node && !button.widget) {
            MenuMgr.setCurrentMenu(button.node);
            menuSearch.suspendEvents();
            menuSearch.setValue('');
            menuSearch.resumeEvents();
        }
    },

    onMenuChange: function (menumgr, oldMenu, newMenu) {
        var menupanel = Ext.ComponentQuery.query('drawer')[0],
            menucontainer = menupanel.down('#menucontainer'),
            backBtn = menupanel.down('#back'),
            buttons;

        if (newMenu.isLeaf()) {
            return;
        }

        buttons = newMenu.getButtons().map(function (node) {
            return Ext.widget(Ext.apply(node.getButtonCfg(), {
                node: node,
                margin: newMenu.getParent() ? '0 10 5 15' : '0 10 5 10'
            }));
        });

        Ext.suspendLayouts();
        menucontainer.removeAll();
        menucontainer.add(buttons);
        Ext.resumeLayouts(true);

        if (newMenu.getButtonCfg()) {
            backBtn.setText(newMenu.getButtonCfg().text);
            backBtn.setGlyph(newMenu.getButtonCfg().glyph);
        } else {
            backBtn.setText(l10n.ns('core').value('defaultBackButtonText'));
            backBtn.setGlyph('xf349@MaterialDesignIcons');
        }

        backBtn.node = newMenu.getParent();
        newMenu.getParent() ? backBtn.show() : backBtn.hide();
        this.highlightCurrentMenuItem();
    },

    onMenuSearchFieldChange: function (searchfield) {
        var value = searchfield.getValue().trim(),
            currentMenu = MenuMgr.getCurrentMenu(),
            triggerEl = searchfield.triggerEl.elements[0];
        menu = [];

        searchfield.isSearchActive = true;

        if (!searchfield.isValid()) {
            return;
        }

        if (Ext.isEmpty(value)) {
            searchfield.isSearchActive = false;
            MenuMgr.setCurrentMenu(currentMenu.getParent());
            triggerEl.removeCls(searchfield.clearCls).addCls(searchfield.searchCls);
            return;
        }

        MenuMgr.menuTraversal(function (node) {
            if (searchfield.maxSearchResult && menu.length >= searchfield.maxSearchResult) {
                return false;
            }

            if (node.isLeaf() && node.getButtonCfg().text.search(new RegExp(value, 'i')) != -1) {
                menu.push(node);
            }
        }, this, currentMenu.isSearchResult ? currentMenu.getParent() : null);

        MenuMgr.setCurrentMenu(Ext.create('App.menu.core.MenuNode', {
            parent: currentMenu.isSearchResult ? currentMenu.getParent() : currentMenu,
            buttons: menu,
            isSearchResult: true
        }));

        triggerEl.removeCls(searchfield.searchCls).addCls(searchfield.clearCls);
    },

    highlightCurrentMenuItem: function () {
        var menucontainer = this.getMenuPanel().down('#menucontainer'),
            currentAlias = this.getCurrentViewAlias(),
            currentButton = menucontainer.down('button[widget=' + currentAlias + ']');

        menucontainer.query('button[widget]').forEach(function (button) {
            button.setUI(MenuMgr.defaultCfg.ui || 'default');
        });
        // Добавляем класс со стрелкой вниз для пунктов меню
        menucontainer.query('button[node]').forEach(function (button) {
            if (!button.node.isLeaf()) {
                button.addCls('x-btn-botm');
            }
        });
        if (currentButton) {
            currentButton.setUI('blue-main-menu-button');
        }
    },

    getCurrentViewAlias: function () {
        var currentView = this.getViewContainer().child();

        if (currentView) {
            var alias = Ext.ClassManager.getAliasesByName(Ext.getClassName(currentView))[0];
            return alias.slice(alias.lastIndexOf('.') + 1);
        }

        return null;
    },

    buildBreadCrumbsPath: function (menuNode) {
        if (menuNode && menuNode.getButtonCfg()) {
            return this.buildBreadCrumbsPath(menuNode.getParent()).concat(menuNode.getButtonCfg().text);
        }

        return [];
    },

    onFunctionButtonClick: function (button) {
        if (button.func) {
            var func = App.util.core.MenuFunctionManager.getMenuFunction(button.func);
            if (func) {
                func(button);
            }
        }
    },

});