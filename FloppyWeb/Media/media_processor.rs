use wasm_bindgen::prelude::*;
use image::{ImageBuffer, Rgba};
use rayon::prelude::*;

#[wasm_bindgen]
pub struct MediaProcessor {
    width: u32,
    height: u32,
    buffer: Vec<u8>,
    filters: Vec<Box<dyn ImageFilter>>,
}

#[wasm_bindgen]
impl MediaProcessor {
    #[wasm_bindgen(constructor)]
    pub fn new(width: u32, height: u32) -> Self {
        MediaProcessor {
            width,
            height,
            buffer: vec![0; (width * height * 4) as usize],
            filters: Vec::new(),
        }
    }

    #[wasm_bindgen]
    pub fn process_frame(&mut self, input_buffer: &[u8]) -> Result<Vec<u8>, JsValue> {
        // Process video/image frame with SIMD acceleration when available
        if is_simd_available() {
            self.process_frame_simd(input_buffer)
        } else {
            self.process_frame_standard(input_buffer)
        }
    }

    #[wasm_bindgen]
    pub fn capture_screenshot(&self) -> Result<Vec<u8>, JsValue> {
        let mut screenshot = self.buffer.clone();
        
        // Apply any active filters
        for filter in &self.filters {
            filter.apply(&mut screenshot, self.width, self.height);
        }

        Ok(screenshot)
    }

    #[wasm_bindgen]
    pub fn optimize_image(&mut self, quality: u8) -> Result<Vec<u8>, JsValue> {
        let img = ImageBuffer::<Rgba<u8>, _>::from_raw(
            self.width,
            self.height,
            self.buffer.clone()
        ).ok_or("Failed to create image buffer")?;

        // Parallel image optimization
        let optimized = img.par_chunks_mut(4)
            .map(|pixel| optimize_pixel(pixel, quality))
            .collect::<Vec<_>>();

        Ok(optimized)
    }
} 