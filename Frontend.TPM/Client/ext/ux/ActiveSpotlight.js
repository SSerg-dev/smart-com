Ext.define('Ext.ux.ActiveSpotlight', {
    extend: 'Ext.Component',
    alias: 'widget.activespotlight',
    mixins: {
        floating: 'Ext.util.Floating',
        observable: 'Ext.util.Observable'
    },

    baseCls: Ext.baseCSSPrefix + 'active-spotlight',
    maskCls: Ext.baseCSSPrefix + 'active-spotlight-mask',
    shadow: false,
    floating: true,
    focusOnToFront: false,
    bringParentToFront: false,
    ariaRole: 'presentation',

    sideNames: ['top', 'right', 'bottom', 'left'],
    childEls: ['topMaskEl', 'rightMaskEl', 'bottomMaskEl', 'leftMaskEl'],

    renderTpl: [
        '<tpl for="$comp.sideNames">',
            '<div id="{parent.id}-{.}MaskEl" role="{parent.role}" class="{parent.$comp.maskCls}"></div>',
        '</tpl>'
    ],

    constructor: function (config) {
        this.ownerCt = this.target = config.target;
        this.addEvents('click');
        this.callParent([config]);
    },

    initEvents: function () {
        this.callParent(arguments);
        this.topMaskEl.on('click', this.onMaskClick, this);
        this.rightMaskEl.on('click', this.onMaskClick, this);
        this.bottomMaskEl.on('click', this.onMaskClick, this);
        this.leftMaskEl.on('click', this.onMaskClick, this);
    },

    onMaskClick: function () {
        this.fireEvent('click', this);
    },

    startUpdateSizeLoop: function () {
        var me = this;

        function step() {
            if (me.rendered && me.isVisible()) {
                me.sizeMask();
                window.requestAnimationFrame(step);
            }
        }

        window.requestAnimationFrame(step);
    },

    sizeMask: function () {
        var container = Ext.getBody();
        this.getEl().setSize(container.getSize()).alignTo(container, 'tl-tl');

        var ctReg = container.getRegion();
        var targetReg = this.target.getRegion();
        var constraintReg = /*this.constraint ||*/ ctReg;
        var spotReg = constraintReg.intersect(targetReg);

        if (spotReg) {
            this.topMaskEl.setRegion(new Ext.util.Region(ctReg.top, spotReg.right, spotReg.top, ctReg.left));
            this.rightMaskEl.setRegion(new Ext.util.Region(ctReg.top, ctReg.right, spotReg.bottom, spotReg.right));
            this.bottomMaskEl.setRegion(new Ext.util.Region(spotReg.bottom, ctReg.right, ctReg.bottom, spotReg.left));
            this.leftMaskEl.setRegion(new Ext.util.Region(spotReg.top, spotReg.left, ctReg.bottom, ctReg.left));
        }
    },

    afterShow: function () {
        this.callParent(arguments);
        this.startUpdateSizeLoop();
    }

});