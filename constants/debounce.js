export function debounce(fn, delay) {
  let timeout = null;
  return function () {
    clearTimeout(timeout);
    let args = arguments;
    let that = this;
    timeout = setTimeout(function () {
      fn.apply(that, args);
    }, delay);
  };
}
