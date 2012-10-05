(function() {
  var __hasProp = Object.prototype.hasOwnProperty;

  Partner.controllerUser = Ember.Object.create({
    m: Partner.controllerMain,
    errorPassword: false,
    errorCompanyname: false,
    errorUsername: false,
    errorEmail: false,
    errorLogin: false,
    errorRegister: false,
    errorUsernameMessage: '',
    errorPasswordMessage: '',
    errorEmailMessage: '',
    errorCompanynameMessage: '',
    errorPlatformAddition: '',
    errorPlatformBackendName: '',
    errorPlatformName: '',
    errorPlatformUrl: '',
    errorPlatformBackendNameMessage: '',
    errorPlatformNameMessage: '',
    errorPlatformUrlMessage: '',
    editEmail: false,
    editUrl: false,
    editPlatformName: false,
    editCompanyname: false,
    editPassword: false,
    settingsChangeError: false,
    settingsChangeErrorText: '',
    init: function() {
      var _this = this;
      this._super();
      this.m.toIndex();
      return this.m.getRequest('/partner/status', {}, function(response) {
        _this.setDefaultParams(response);
        _this.m.toIndex();
        if (response.authenticated) return _this.setUser(response);
      });
    },
    current: Ember.Object.create({
      loaded: false,
      authenticated: false,
      partnerId: '',
      url: '',
      platforms: [],
      platform: '',
      notAuthenticated: Em.computed(function() {
        return !this.get('authenticated');
      }).property('authenticated'),
      setData: function(data) {
        var index, item, _results;
        _results = [];
        for (item in data) {
          if (!__hasProp.call(data, item)) continue;
          index = data[item];
          _results.push(this.set(item, index));
        }
        return _results;
      }
    }),
    login: function(data) {
      var _this = this;
      if (data.remember != null) {
        data.remember = true;
      } else {
        data.remember = false;
      }
      return this.m.postRequest('/partner/login', data, function(response) {
        if (response.authenticated) {
          _this.set('errorLogin', false);
          _this.setUser(response);
          return Partner.controllerGames.set('platforms', response.platforms);
        } else {
          return _this.set('errorLogin', true);
        }
      });
    },
    setCurrentPlatform: function(id) {
      var platform, platforms, user, _i, _len, _results;
      user = this.get('current');
      platforms = user.platforms;
      console.log('Current platform: ' + id + ' Platforms: ' + platforms.length);
      _results = [];
      for (_i = 0, _len = platforms.length; _i < _len; _i++) {
        platform = platforms[_i];
        if (platform.id === id) {
          this.set('platform', platform);
          _results.push(console.log('Current platform setted.'));
        } else {
          _results.push(void 0);
        }
      }
      return _results;
    },
    setDefaultParams: function(response) {
      return this.m.set('resultMessage', response.resultMessage);
    },
    setUser: function(response) {
      var user;
      user = this.get('current');
      user.set('authenticated', true);
      user.setData(response.user);
      return this.set('current', user);
    },
    logout: function() {
      var user;
      user = this.get('current');
      user.set('authenticated', false);
      this.set('current', user);
      return this.m.getRequest('/partner/logout');
    },
    changeAPIKey: function() {
      var _this = this;
      return this.m.getRequest('/partner/change_api_key', {
        platform_id: Partner.controllerUser.current.platform.id
      }, function(response) {
        var user;
        user = _this.get('current');
        user.set('api_key', response.api_key);
        return _this.set('current', user);
      });
    },
    changeData: function(data) {
      var text,
        _this = this;
      text = "";
      return this.m.postRequest('/partner/edit', data, function(response) {
        var user;
        user = _this.get('current');
        if (response.authenticated) {
          user.setData(response.user);
          _this.set('settingsChangeError', false);
          Partner.controllerUser.set('editEmail', false);
          Partner.controllerUser.set('editCompanyname', false);
          Partner.controllerUser.set('editUrl', false);
          Partner.controllerUser.set('editPassword', false);
          $('#enterPassword').modal('hide');
          return _this.set('current', user);
        } else {
          response.errors.forEach(function(error) {
            return text += error.message + "\n";
          });
          _this.set('settingsChangeError', true);
          return _this.set('settingsChangeErrorText', text);
        }
      });
    },
    changePlatformData: function(data) {
      var text,
        _this = this;
      text = "";
      return this.m.postRequest('/partner/edit_platform', data, function(response) {
        var user;
        user = _this.get('current');
        if (response.authenticated) {
          user.setData(response.user);
          _this.set('settingsChangeError', false);
          Partner.controllerUser.set('editPlatformName', false);
          Partner.controllerUser.set('editUrl', false);
          $('#enterPassword').modal('hide');
          return _this.set('current', user);
        } else {
          response.errors.forEach(function(error) {
            return text += error.message + "\n";
          });
          _this.set('settingsChangeError', true);
          return _this.set('settingsChangeErrorText', text);
        }
      });
    },
    register: function(data) {
      var _this = this;
      $('#register').bind('hidden', function() {
        _this.set('errorRegister', false);
        _this.set('errorUsername', false);
        _this.set('errorPassword', false);
        _this.set('errorCompanyname', false);
        return _this.set('errorEmail', false);
      });
      return this.m.postRequest('/partner/register', data, function(response) {
        var user;
        _this.set('errorRegister', false);
        _this.set('errorUsername', false);
        _this.set('errorPassword', false);
        _this.set('errorCompanyname', false);
        _this.set('errorEmail', false);
        if (response.status === "ok") {
          $('#register').modal('hide');
          user = _this.get('current');
          user.set('authenticated', true);
          user.setData(response.user);
          return _this.set('current', user);
        } else {
          _this.set('errorRegister', true);
          return response.errors.forEach(function(error) {
            switch (error.error) {
              case "username":
                _this.set('errorUsername', true);
                return _this.set('errorUsernameMessage', error.message);
              case "company_name":
                _this.set('errorCompanyname', true);
                return _this.set('errorCompanynameMessage', error.message);
              case "password":
                _this.set('errorPassword', true);
                return _this.set('errorPasswordMessage', error.message);
              case "email":
                _this.set('errorEmail', true);
                return _this.set('errorEmailMessage', error.message);
            }
          });
        }
      });
    },
    addPlatform: function(data) {
      var _this = this;
      $('#addPlatform').bind('hidden', function() {
        _this.set('errorPlatformAddition', false);
        _this.set('errorPlatformBackendName', false);
        _this.set('errorPlatformName', false);
        return _this.set('errorPlatformUrl', false);
      });
      return this.m.postRequest('/partner/create_a_partner', data, function(response) {
        var user;
        _this.set('errorPlatformAddition', false);
        _this.set('errorPlatformBackendName', false);
        _this.set('errorPlatformName', false);
        _this.set('errorPlatformUrl', false);
        if (response.status === "ok") {
          $('#addPlatform').modal('hide');
          user = _this.get('current');
          user.set('authenticated', true);
          user.setData(response.user);
          _this.set('current', user);
          return Partner.routeManager.set('location', 'platforms');
        } else {
          _this.set('errorPlatformAddition', true);
          return response.errors.forEach(function(error) {
            switch (error.error) {
              case "platformBackendName":
                _this.set('errorPlatformBackendName', true);
                return _this.set('errorPlatformBackendNameMessage', error.message);
              case "platformName":
                _this.set('errorPlatformName', true);
                return _this.set('errorPlatformNameMessage', error.message);
              case "platformUrl":
                _this.set('errorPlatformUrl', true);
                return _this.set('errorPlatformUrlMessage', error.message);
            }
          });
        }
      });
    }
  });

}).call(this);
