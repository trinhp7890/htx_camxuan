angular
  .module('app')
  .factory('permissions', permissions)
  .directive('li', navMenuCollapse)
  .directive('includeReplace', includeReplace)
  .directive('a', preventClickDirective)
  .directive('a', bootstrapCollapseDirective)
  .directive('a', navigationDirective)
  .directive('button', layoutToggleDirective)
  .directive('a', layoutToggleDirective)
  .directive('button', collapseMenuTogglerDirective)
  .directive('div', bootstrapCarouselDirective)
  .directive('toggle', bootstrapTooltipsPopoversDirective)
  .directive('tab', bootstrapTabsDirective)
  .directive('button', cardCollapseDirective)
  .directive('fileinput', fileinput)
  .directive('myEnter', myEnter)
  .directive('hasPermission', hasPermission)
  .directive('number',number);

/**
 * 
 * @param {*} $rootScope 
 */
permissions.$inject = ['$rootScope'];

function permissions($rootScope) {
  var permissionList = [];
  return {
    setPermissions: function (permissions) {
      permissionList = permissions;
      $rootScope.$broadcast('permissionsChanged');
    },
    hasPermission: function (permission) {
      permission = permission.trim();
      return permissionList.some(item => {
        if (typeof item !== 'string') { // item.Name is only used because when I called setPermission, I had a Name property
          return false;
        }

        var pers = permission.split(',');

        return pers.indexOf(item.trim()) > -1
        // return item.trim() === permission;
      });
    }
  };
}

function hasPermission(permissions) {
  return {
    link: function (scope, element, attrs) {
      if (!_.isString(attrs.hasPermission)) {
        throw 'hasPermission value must be a string'
      }
      var value = attrs.hasPermission.trim();
      // var notPermissionFlag = value[0] === '!';
      // if (notPermissionFlag) {
      //   value = value.slice(1).trim();
      // }

      function toggleVisibilityBasedOnPermission() {
        var hasPermission = permissions.hasPermission(value);
        if (hasPermission /*&& !notPermissionFlag || !hasPermission && notPermissionFlag*/) {
          element[0].style.display = 'block';
        } else {
          element[0].style.display = 'none';
        }
      }

      toggleVisibilityBasedOnPermission();
      scope.$on('permissionsChanged', toggleVisibilityBasedOnPermission);
    }
  };
}

function myEnter() {
  var directive = {
    restrict: 'A',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    element.bind("keydown keypress", function (event) {
      if (event.which === 13) {
        scope.$apply(function () {
          scope.$eval(attrs.myEnter);
        });

        event.preventDefault();
      }
    });
  }
}
function fileinput() {
  console.log('fileinput');
  return {
    scope: {
      fileinput: "=",
      filepreview: "=",
    },
    link: function (scope, element, attributes) {
      
      element.bind("change", function (changeEvent) {
        scope.fileinput = changeEvent.target.files[0];
        if (typeof changeEvent.target.files[0] != 'undefined') {
          if (changeEvent.target.files[0].size <= 0) {
            alert("Ảnh bạn chọn <= 0MB");
          } else if (changeEvent.target.files[0].type != 'image/jpeg') {
            alert("Chương trình chỉ hỗ trợ định dạng ảnh jpg");
          } else {
            if (document.getElementById('anh_test') == null) {
              var reader = new FileReader();
              reader.onload = function (loadEvent) {
                scope.$apply(function () {
                  scope.filepreview = loadEvent.target.result;
                });
              }
              reader.readAsDataURL(scope.fileinput);
            } else {
              document.getElementById('anh_test').style.visibility = "hidden";
              document.getElementById('anh_test').style.height = "0px";
              var reader = new FileReader();
              reader.onload = function (loadEvent) {
                scope.$apply(function () {
                  scope.filepreview = loadEvent.target.result;
                });
              }
              reader.readAsDataURL(scope.fileinput);
            }
          }
        } else {
          scope.filepreview = "";
        }
      });
      element.on('$destroy', function() {
        element.off();
      });
    }
  }
}

function includeReplace() {
  var directive = {
    require: 'ngInclude',
    restrict: 'A',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    element.replaceWith(element.children());
  }
}

//Prevent click if href="#"
function preventClickDirective() {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    if (attrs.href === '#') {
      element.on('click', function (event) {
        event.preventDefault();
      });
    }
  }
}

//Bootstrap Collapse
function bootstrapCollapseDirective() {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    if (attrs.toggle == 'collapse') {
      element.attr('href', 'javascript;;').attr('data-target', attrs.href.replace('index.html', ''));
    }
  }
}

/**
 * 
 */
function navMenuCollapse() {
  var directive = {
    restrict: 'E',
    link: link
  };
  return directive;

  function link(scope, element, attrs) {
    if (element.data('toggle') === 'collapse') {
      element.children('a').on('click', function (event) {
        element.toggleClass("collapsed");
        element.children('a').removeClass('collapsed');
        element.children('ul.sub-menu').toggleClass("collapse");
      });
    }
  }
}

/**
 * @desc Genesis main navigation - Siedebar menu
 * @example <li class="nav-item nav-dropdown"></li>
 */
function navigationDirective() {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    if (element.hasClass('nav-dropdown-toggle') && angular.element('body').width() > 782) {
      element.on('click', function () {
        if (!angular.element('body').hasClass('compact-nav')) {
          element.parent().toggleClass('open').find('.open').removeClass('open');
        }
      });
    } else if (element.hasClass('nav-dropdown-toggle') && angular.element('body').width() < 783) {
      element.on('click', function () {
        element.parent().toggleClass('open').find('.open').removeClass('open');
      });
    }
  }
}

//Dynamic resize .sidebar-nav
sidebarNavDynamicResizeDirective.$inject = ['$window', '$timeout'];

function sidebarNavDynamicResizeDirective($window, $timeout) {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {

    if (element.hasClass('sidebar-nav') && angular.element('body').hasClass('fixed-nav')) {
      var bodyHeight = angular.element(window).height();
      scope.$watch(function () {
        var headerHeight = angular.element('header').outerHeight();

        if (angular.element('body').hasClass('sidebar-off-canvas')) {
          element.css('height', bodyHeight);
        } else {
          element.css('height', bodyHeight - headerHeight);
        }
      })

      angular.element($window).bind('resize', function () {
        var bodyHeight = angular.element(window).height();
        var headerHeight = angular.element('header').outerHeight();
        var sidebarHeaderHeight = angular.element('.sidebar-header').outerHeight();
        var sidebarFooterHeight = angular.element('.sidebar-footer').outerHeight();

        if (angular.element('body').hasClass('sidebar-off-canvas')) {
          element.css('height', bodyHeight - sidebarHeaderHeight - sidebarFooterHeight);
        } else {
          element.css('height', bodyHeight - headerHeight - sidebarHeaderHeight - sidebarFooterHeight);
        }
      });
    }
  }
}

//LayoutToggle
layoutToggleDirective.$inject = ['$interval'];

function layoutToggleDirective($interval) {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    element.on('click', function () {

      if (element.hasClass('sidebar-toggler')) {
        angular.element('body').toggleClass('sidebar-hidden');
      }

      if (element.hasClass('aside-menu-toggler')) {
        angular.element('body').toggleClass('aside-menu-hidden');
      }
    });
  }
}

//Collapse menu toggler
function collapseMenuTogglerDirective() {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    element.on('click', function () {
      if (element.hasClass('navbar-toggler') && !element.hasClass('layout-toggler')) {
        angular.element('body').toggleClass('sidebar-mobile-show')
      }
    })
  }
}

//Bootstrap Carousel
function bootstrapCarouselDirective() {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    if (attrs.ride == 'carousel') {
      element.find('a').each(function () {
        $(this).attr('data-target', $(this).attr('href').replace('index.html', '')).attr('href', 'javascript;;')
      });
    }
  }
}

//Bootstrap Tooltips & Popovers
function bootstrapTooltipsPopoversDirective() {
  var directive = {
    restrict: 'A',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    if (attrs.toggle == 'tooltip') {
      angular.element(element).tooltip();
    }
    if (attrs.toggle == 'popover') {
      angular.element(element).popover();
    }
  }
}

//Bootstrap Tabs
function bootstrapTabsDirective() {
  var directive = {
    restrict: 'A',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    element.click(function (e) {
      e.preventDefault();
      angular.element(element).tab('show');
    });
  }
}

//Card Collapse
function cardCollapseDirective() {
  var directive = {
    restrict: 'E',
    link: link
  }
  return directive;

  function link(scope, element, attrs) {
    if (attrs.toggle == 'collapse' && element.parent().hasClass('card-actions')) {

      if (element.parent().parent().parent().find('.card-body').hasClass('in')) {
        element.find('i').addClass('r180');
      }

      var id = 'collapse-' + Math.floor((Math.random() * 1000000000) + 1);
      element.attr('data-target', '#' + id)
      element.parent().parent().parent().find('.card-body').attr('id', id);

      element.on('click', function () {
        element.find('i').toggleClass('r180');
      })
    }
  }
}
//number
function number() {
  return {
      require: 'ngModel',
      restrict: 'A',
      link: function (scope, element, attrs, ctrl) {
          ctrl.$parsers.push(function (input) {
              if (input == undefined) return ''
              var inputNumber = input.toString().replace(/[^0-9]/g, '');
              if (inputNumber != input) {
                  ctrl.$setViewValue(inputNumber);
                  ctrl.$render();
              }
              return inputNumber;
          });
      }
  };
}