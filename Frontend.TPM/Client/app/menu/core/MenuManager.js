Ext.define('App.menu.core.MenuManager', {
    extend: 'App.AppComponent',
    alternateClassName: 'MenuMgr',
    singleton: true,
    id: 'menumgr',

    // Конфиг для кнопок меню, который будет применён по умолчанию.
    defaultCfg: {
        xtype: 'button',
        textAlign: 'left',
        ui: 'main-menu-button',
        menuAlign: 'tr-tl?'
    },

    constructor: function () {
        this.addEvents('menuchange');
        this.callParent(arguments);
        this.menuConfig = [];
    },

    defineMenu: function (config) {
        this.menuConfig = this.menuConfig.concat(config);
    },

    init: function () {
        if (this.menuConfig) {
            var availableMenuConfig = this.getAvailableMenu(this.menuConfig),
                oldMenu = this.currentMenu;
            this.currentMenu = this.buildMenu(availableMenuConfig);
            this.fireEvent('menuchange', this, oldMenu, this.currentMenu);
        }
    },

    getAvailableMenu: function (items) {
        function reducer(result, item) {
            if (item.hasOwnProperty('children')) {
                var children = this.getAvailableMenu(item.children);

                if (!Ext.isEmpty(children)) {
                    return result.concat(Ext.applyIf({ children: children }, item));
                }
            } else {
                if (this.isAvailable(item)) {
                    return result.concat(Ext.clone(item));
                }
            }

            return result;
        };

        return items.reduce(reducer.bind(this), []);
    },

    isAvailable: function (config) {
        var hasValidRolesProperty = config.hasOwnProperty('roles') && Ext.isArray(config.roles);

        if (!hasValidRolesProperty) {
            return true;
        }

        var currentRole = App.UserInfo.getCurrentRole();

        if (!currentRole) {
            return false;
        }
        
        if (config.hasOwnProperty('exeptRoles') && config.exeptRoles) {
            return config.roles.indexOf(currentRole.SystemName) == -1;
        }

        return config.roles.indexOf(currentRole.SystemName) !== -1;
    },

    // Возвращает дерево меню, построенное на основе конфига.
    buildMenu: function (subtree, parentNode, nodeBtnCfg) {
        var node = Ext.create('App.menu.core.MenuNode', {
            parent: parentNode,
            buttonCfg: Ext.applyIf(nodeBtnCfg, this.defaultCfg)
        });

        node.buttons = subtree.map(function (btnCfg) {
            if (btnCfg.children) {
                var tmpCfg = {};
                Ext.Object.each(btnCfg, function (key, value) {
                    if (key != 'children') {
                        tmpCfg[key] = value;
                    }
                });
                return this.buildMenu(btnCfg.children, node, tmpCfg);
            }

            return Ext.create('App.menu.core.MenuNode', {
                parent: node,
                buttonCfg: Ext.applyIf(btnCfg, this.defaultCfg)
            });
        }, this);

        return node;
    },

    // Устанавливает текущее меню.
    setCurrentMenu: function (menu) {
        if (menu) {
            var oldValue = this.currentMenu;
            this.currentMenu = menu;
            this.fireEvent('menuchange', this, oldValue, menu);
        }
    },

    // Возвращает текущий узел из дерева меню.
    getCurrentMenu: function () {
        return this.currentMenu;
    },

    // Совершает обход дерева меню и вызывает callback для каждого узла.
    menuTraversal: function (callback, scope, node) {
        node = node || this.getCurrentMenu();

        if (Ext.callback(callback, scope || this, [node]) === false) {
            return;
        }

        if (!node.isLeaf()) {
            node.buttons.forEach(function (item) {
                this.menuTraversal(callback, scope, item);
            }, this);
        }
    }
});