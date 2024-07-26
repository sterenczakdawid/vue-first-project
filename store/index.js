export const state = () => ({
  lang: "pl",
});

export const getters = {
  getLang(state) {
    return state.lang;
  },
};

export const mutations = {
  setLang(state, lang) {
    state.lang = lang;
  },
};

export const actions = {
  setLang(context, lang) {
    context.commit("setLang", lang);
  },
};
