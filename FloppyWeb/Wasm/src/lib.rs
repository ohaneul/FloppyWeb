use wasm_bindgen::prelude::*;
use web_sys::{HtmlCanvasElement, CanvasRenderingContext2d};

#[wasm_bindgen]
pub struct ImageProcessor {
    context: CanvasRenderingContext2d,
    width: u32,
    height: u32,
}

#[wasm_bindgen]
impl ImageProcessor {
    #[wasm_bindgen(constructor)]
    pub fn new(canvas: HtmlCanvasElement) -> Result<ImageProcessor, JsValue> {
        let context = canvas
            .get_context("2d")?
            .unwrap()
            .dyn_into::<CanvasRenderingContext2d>()?;

        Ok(ImageProcessor {
            context,
            width: canvas.width(),
            height: canvas.height(),
        })
    }

    // Fast image processing using WASM
    #[wasm_bindgen]
    pub fn apply_filter(&self, filter_type: &str) -> Result<(), JsValue> {
        let image_data = self.context
            .get_image_data(0.0, 0.0, self.width as f64, self.height as f64)?;
        
        let data = image_data.data();
        let mut filtered = vec![0; data.len()];

        // Process pixels in parallel
        data.chunks(4)
            .zip(filtered.chunks_mut(4))
            .for_each(|(src, dst)| {
                match filter_type {
                    "grayscale" => self.apply_grayscale(src, dst),
                    "blur" => self.apply_blur(src, dst),
                    _ => dst.copy_from_slice(src),
                }
            });

        Ok(())
    }
} 