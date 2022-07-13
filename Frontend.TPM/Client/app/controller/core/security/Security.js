// ----------------------------------------------------------------------
// <copyright file="Security.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.controller.core.security.Security', {
    extend: 'Ext.app.Controller',

    refs: [{
        ref: 'rolesView',
        selector: 'changerolewindow rolesview'
    }, {
        ref: 'roleButton',
        selector: 'toptoolbar #rolebutton'
    }, {
        ref: 'userButton',
        selector: 'toptoolbar #userbutton'
    }, {
        ref: 'viewport',
        selector: 'viewport'
    }, {
        ref: 'changeRoleWindow',
        selector: 'changerolewindow',
        xtype: 'changerolewindow',
        autoCreate: true
    }, {
        ref: 'userProfileWindow',
        selector: 'userprofilewindow',
        xtype: 'userprofilewindow',
        autoCreate: true
    }, {
        ref: 'logInWindow',
        selector: 'loginwindow',
        xtype: 'loginwindow',
        autoCreate: true
    }],

    init: function () {
        this.listen({
            component: {
                'changerolewindow rolesview': {
                    selectionchange: this.onRoleSelectionChange
                },
                'toptoolbar #rolebutton': {
                    click: this.onRoleButtonClick
                },
                'toptoolbar #userbutton': {
                    click: this.onUserButtonClick
                },
                'changerolewindow #ok': {
                    click: this.onOkButtonClick
                },
                'changerolewindow #cancel': {
                    click: this.onCancelButtonClick
                },
                'loginwindow #ok': {
                    click: this.onLogInButtonClick
                },
                '#exitbutton': {
                    click: this.onLogOutButtonClick
                },
                //'userprofilewindow #changepassword': {
                //    click: this.onChangePasswordButtonClick
                //},
                '#securityuserpasswindow #ok': {
                    click: this.onOkChangeUserPassClick
                },
                'toptoolbar': {
                    afterrender: this.afterrenderToolBar
                }
            },
            app: {
                '#userinfo': {
                    beforeload: this.onUserInfoBeforeload,
                    load: this.onUserInfoLoad,
                    login: this.onLogIn
                }
            }
        });
    },

    onLogOutButtonClick: function (button) {
        Ext.Msg.show({
            title: l10n.ns('core').value('confirmTitle'),
            msg: l10n.ns('core').value('exitConfirmMessage'),
            fn: function (buttonId) {
                if (buttonId === 'yes') {
                    App.UserInfo.logOut();
                }
            },
            scope: this,
            icon: Ext.Msg.QUESTION,
            buttons: Ext.Msg.YESNO,
            buttonText: {
                yes: l10n.ns('core', 'buttons').value('exit'),
                no: l10n.ns('core', 'buttons').value('cancel')
            }
        });
    },

    onRoleSelectionChange: function (rolesView, oldRole, newRole) {
        this.getChangeRoleWindow()
            .down('#ok')
            .setDisabled(!rolesView.hasSelectedRole);
    },

    onRoleButtonClick: function (button) {
        var role = App.UserInfo.getCurrentRole();

        this.getChangeRoleWindow().show();

        if (role) {
            this.getRolesView().setRole(role.SystemName);
        }
    },

    onLogInButtonClick: function (button) {
        var window = this.getLogInWindow(),
            form = window.down('form');

        window.setLoading(true);

        if (form.isValid()) {
            var values = form.getValues();
            App.UserInfo.logIn(values.login, values.password);
        } else {
            window.setLoading(false);
        }

        var passField = form.down('textfield[name=password]');
        passField.reset();
    },

    onLogIn: function (ctx) {
        var window = this.getLogInWindow();
        if (window) {
            window.setLoading(false);

            if (App.UserInfo.isAuthenticated) {
                window.close();
                this.getView('Viewport').create();
                this.onUserInfoLoad(ctx);
            }
        }
    },

    onUserInfoBeforeload: function (ctx) {
        var viewport = this.getViewport();
        if (viewport) {
            viewport.setLoading(true);
        }
    },

    onUserInfoLoad: function (ctx) {
        var viewport = this.getViewport();
        if (viewport) {
            viewport.setLoading(false);
        }

        //if (!App.UserInfo.isAuthenticated) {
        //    this.getLogInWindow().show();
        //} else {
        var role = App.UserInfo.getCurrentRole(),
            username = App.UserInfo.getUserName(),
            rolebutton = this.getRoleButton(),
            userbutton = this.getUserButton();

        if (!Ext.isEmpty(role) && !Ext.isEmpty(rolebutton)) {
            rolebutton.setText(role.DisplayName);
        }

        if (!Ext.isEmpty(userbutton)) {
            userbutton.setText(username);
        }

        Ext.getCmp('viewcontainer').removeAll(true);
        MenuMgr.init();
        //}
    },

    onOkButtonClick: function (button) {
        var role = this.getRolesView().getRole();

        if (role) {
            App.UserInfo.setCurrentRole(role);
        }

        this.getChangeRoleWindow().close();

        // Очистка меню "Последние использованные"
        var recentMenuItems = Ext.ComponentQuery.query('recentmenuitems');
        if (recentMenuItems && recentMenuItems.length > 0) {
            recentMenuItems[0].removeAll();
        }
        // Очистка нижней панели "Задачи"
        var store = Ext.data.StoreManager.get("userloophandlerstore");
        if (store) {
            store.load();
        }
    },

    onCancelButtonClick: function (button) {
        this.getChangeRoleWindow().close();
    },

    onUserButtonClick: function (button) {
        var window = this.getUserProfileWindow();

        if (App.UserInfo.getAuthSourceType() == 'ActiveDirectory') {
            window.down('#changepassword').hide();
        }

        window.show();
    },

    //onChangePasswordButtonClick: function (button) {
    //    Ext.widget('passwordchangingwindow', {
    //        id: 'securityuserpasswindow'
    //    }).show();
    //},

    onOkChangeUserPassClick: function (button) {
        var window = button.up('window'),
            form = window.down('editorform');

        if (!form.getForm().isValid()) {
            return;
        }

        window.setLoading(true);
        Ext.Ajax.request({
            method: 'POST',
            url: 'api/Security/ChangePassword',
            scope: this,
            params: {
                Password: form.down('passwordfield').getValue()
            },
            success: function (response, opts) {
                window.setLoading(false);
                window.close();
            },
            failure: function () {
                window.setLoading(false);
            }
        });
    },
    afterrenderToolBar: function (toolbar) {
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        settingStore.load();
        if (settingStore.data.length == 0) {
            settingStore.add({ name: 'mode', value: 0 });
            settingStore.sync();
        }
    },
    changeMode: function (mode) {
        debugger;
    }
});