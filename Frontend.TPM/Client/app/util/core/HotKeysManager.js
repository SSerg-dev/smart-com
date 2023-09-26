Ext.define('App.Util.core.HotKeysManager', {
    alternateClassName: 'KeysMngr',
    statics: {
        bindKeys: function () {
            //переключение между полями формы
            Ext.override(Ext.form.field.Base, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    if (me.keyMap) {
                        me.keyMap.enable();
                        return;
                    }
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        fn: KeysMngr.onEnter
                    });
                }
            });
            // Форма редактирования - отмена/подтверждение
            Ext.override(App.view.core.common.EditableDetailForm, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    if (me.keyMap) {
                        me.keyMap.enable();
                        return;
                    }
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.ESC,
                        fn: function () {
                            var button = me.down('#cancel');
                            button.fireEvent('click', button);
                        }
                    }, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        ctrl: true,
                        fn: function () {
                            var button = me.down('#select');
                            if (!button) {
                                button = me.down('#ok');
                            };
                            button.fireEvent('click', button);
                        }
                    }]);
                }
            });
            // Окно редактирования - отмена/подтверждение
            Ext.override(App.view.core.common.EditorWindow, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.ESC,
                        fn: function () {
                            var button = me.down('#cancel');
                            button.fireEvent('click', button);
                        }
                    }, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        ctrl: true,
                        fn: function () {
                            var button = me.down('#select');
                            if (!button) {
                                button = me.down('#ok');
                            };
                            button.fireEvent('click', button);
                        }
                    }]);
                }
            });
            // Поле выбора даты, времени - выбор текущей даты/открытие пикера/ очистка поля
            Ext.override(App.view.core.common.DatetimeField, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    if (me.keyMap) {
                        me.keyMap.enable();
                        return;
                    }
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.DOWN,
                        ctrl: true,
                        fn: me.onTrigger1Click      // выбрать текущую дату
                    }, {
                        scope: me,
                        key: Ext.EventObject.UP,
                        ctrl: true,
                        fn: me.onTrigger2Click      //открыть пикер
                    }, {
                        scope: me,
                        key: Ext.EventObject.BACKSPACE,
                        ctrl: true,
                        fn: me.onTrigger3Click      // очистить поле
                    }, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        fn: KeysMngr.onEnter
                    }]);
                }
            });
            // Поле выбора даты - открытие пикера/ очистка поля
            Ext.override(Ext.form.field.Date, {
                show: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.UP,
                        ctrl: true,
                        fn: function () {
                            this.onDownArrow();
                        }      //открыть пикер
                    }, {
                        scope: me,
                        key: Ext.EventObject.BACKSPACE,
                        ctrl: true,
                        fn: function() {
                            this.reset();
                        }     // очистить поле
                    }, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        fn: KeysMngr.onEnter
                    }]);
                }
            });
            // Поле выбора записи - открытие пикера/ очистка поля
            Ext.override(App.view.core.common.SearchField, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    if (me.keyMap) {
                        me.keyMap.enable();
                        return;
                    }
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.UP,
                        ctrl: true,
                        fn: me.onTrigger1Click      // открыть пикер
                    }, {
                        scope: me,
                        key: Ext.EventObject.BACKSPACE,
                        ctrl: true,
                        fn: me.onTrigger2Click      // очистить поле
                    }, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        fn: KeysMngr.onEnter
                    }]);
                }
            });
            // Грид - выбор записи для serchfield'a
            Ext.override(Ext.grid.View, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    if (me.keyMap) {
                        me.keyMap.enable();
                        return;
                    }
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        ctrl: true,
                        fn: function () {
                            var selector = me.up('selectorwindow');
                            if (selector) {
                                var button = selector.down('#select');
                                if (!button) {
                                    button = me.down('#ok');
                                };
                                button.fireEvent('click', button);
                            }
                        }
                    }]);
                }
            });
            // Расширенный фильтр - закрыть/применить
            Ext.override(App.view.core.filter.ExtendedSelectionFilter, {
                afterRender: function () {
                    this.callParent(arguments);
                    this.bindKeyMap();
                },
                bindKeyMap: function () {
                    var me = this;
                    if (me.keyMap) {
                        me.keyMap.enable();
                        return;
                    }
                    me.keyMap = Ext.create('Ext.util.KeyMap', me.el, [{
                        scope: me,
                        key: Ext.EventObject.ESC,
                        fn: function () {
                            var window = me.up('extfilter'),
                                button = window.down('#cancel');
                            button.fireEvent('click', button);
                        }
                    }, {
                        scope: me,
                        key: Ext.EventObject.ENTER,
                        ctrl: true,
                        fn: function () {
                            var window = me.up('extfilter'),
                                button = window.down('#apply');
                            button.fireEvent('click', button);
                        }
                    }]);
                }
            });
        },
        // Переключение между полями формы
        onEnter: function () {
            var curentForm = this.up('form');
            var nextfield = this.nextNode('field');     //nextSibling - не видит вторую колонку
            if (nextfield && nextfield.xtype != "singlelinedisplayfield" && nextfield.xtype != "passwordfield") {
                var nextFieldForm = nextfield.up('form');
                if (nextFieldForm && curentForm.id == nextFieldForm.id) {       // т.к. nextNode перебирает все узлы, проверям, что следующее поле в этой же форме
                    nextfield.focus(true, 100);
                }
            };
        }
    }
})